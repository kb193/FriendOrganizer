Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Model
Imports Prism.Commands
Imports Prism.Events

Public Class FriendDetailViewModel
    Inherits DetailViewModelBase
    Implements IFriendDetailViewModel

    Private _friendRepository As IFriendRepository
    Private _programmingLanguageLookupDataService As IProgrammingLanguageLookupDataService
    Private _friend As FriendWrapper
    Private _selectedPhoneNumber As PhoneNumberWrapper

    Public Property [Friend] As FriendWrapper
        Get
            Return _friend
        End Get
        Set(value As FriendWrapper)
            _friend = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property SelectedPhoneNumber As PhoneNumberWrapper
        Get
            Return _selectedPhoneNumber
        End Get
        Set(value As PhoneNumberWrapper)
            _selectedPhoneNumber = value
            OnPropertyChanged()
            DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
        End Set
    End Property

    Public Property PhoneNumbers As New ObservableCollection(Of PhoneNumberWrapper)
    Public Property ProgrammingLanguages As New ObservableCollection(Of LookupItem)

    Public Property AddPhoneNumberCommand As ICommand
    Public Property RemovePhoneNumberCommand As ICommand

    Sub New(friendRepo As IFriendRepository,
            eventAggregator As IEventAggregator,
            messageDialogService As IMessageDialogService,
            programmingLanguageLookupDataService As IProgrammingLanguageLookupDataService)
        MyBase.New(eventAggregator, messageDialogService)
        Me._friendRepository = friendRepo
        Me._programmingLanguageLookupDataService = programmingLanguageLookupDataService

        AddPhoneNumberCommand = New DelegateCommand(AddressOf OnAddPhoneNumberExecute)
        RemovePhoneNumberCommand = New DelegateCommand(AddressOf OnRemovePhoneNumberExecute, AddressOf OnRemovePhoneNumberCanExecute)

        Me._eventAggregator.GetEvent(Of AfterCollectionSavedEvent).Subscribe(AddressOf AfterCollectionSaved)
    End Sub

    Public Overrides Async Function LoadAsync(friendId As Integer) As Task
        Dim loadFriend = If(friendId > 0, Await _friendRepository.GetByIdAsync(friendId), CreateNewFriend())

        Me.Id = loadFriend.Id

        InitializeFriend(loadFriend)
        InitializeFriendPhoneNumbers(loadFriend.PhoneNumbers)
        Await LoadProgrammingLanguagesLookupAsync()
    End Function

    Private Sub InitializeFriendPhoneNumbers(phoneNumbers As ICollection(Of FriendPhoneNumber))
        For Each number In Me.PhoneNumbers
            RemoveHandler number.PropertyChanged, AddressOf PhoneNumberWrapper_PropertyChanged
        Next

        Me.PhoneNumbers.Clear()

        For Each number In phoneNumbers
            Dim phoneWrapper As New PhoneNumberWrapper(number)
            Me.PhoneNumbers.Add(phoneWrapper)
            AddHandler phoneWrapper.PropertyChanged, AddressOf PhoneNumberWrapper_PropertyChanged
        Next

    End Sub

    Private Async Function LoadProgrammingLanguagesLookupAsync() As Task
        ProgrammingLanguages.Clear()
        ProgrammingLanguages.Add(New NullLookupItem With {.DisplayMember = " - "})
        Dim lookups = Await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync
        For Each item In lookups
            ProgrammingLanguages.Add(item)
        Next
    End Function

    Private Sub InitializeFriend(loadFriend As [Friend])
        Me.[Friend] = New FriendWrapper(loadFriend)
        AddHandler Me.[Friend].PropertyChanged, Sub(s, e)
                                                    If HasChanges = False Then
                                                        HasChanges = _friendRepository.HasChanges
                                                    End If
                                                    If e.PropertyName = NameOf(Me.Friend.HasErrors) Then
                                                        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
                                                    End If
                                                    If e.PropertyName = NameOf([Friend].FirstName) Or e.PropertyName = NameOf([Friend].LastName) Then
                                                        SetTitle()
                                                    End If
                                                End Sub
        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
        If Me.[Friend].Id = 0 Then
            Me.[Friend].FirstName = ""
        End If
        SetTitle()
    End Sub

    Private Function CreateNewFriend() As [Friend]
        Dim newFriend As New [Friend]
        _friendRepository.Add(newFriend)
        Return newFriend
    End Function

    Private Async Sub AfterCollectionSaved(args As AfterCollectionSavedEventArgs)
        If args.ViewModelName = NameOf(ProgrammingLanguageDetailViewModel) Then
            Await LoadProgrammingLanguagesLookupAsync()
        End If
    End Sub

    Private Sub OnAddPhoneNumberExecute()
        Dim phoneWrapper As New PhoneNumberWrapper(New FriendPhoneNumber)
        AddHandler phoneWrapper.PropertyChanged, AddressOf PhoneNumberWrapper_PropertyChanged
        PhoneNumbers.Add(phoneWrapper)
        [Friend].Model.PhoneNumbers.Add(phoneWrapper.Model)
        phoneWrapper.Number = ""
    End Sub

    Private Sub OnRemovePhoneNumberExecute()
        RemoveHandler SelectedPhoneNumber.PropertyChanged, AddressOf PhoneNumberWrapper_PropertyChanged
        _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model)
        PhoneNumbers.Remove(SelectedPhoneNumber)
        SelectedPhoneNumber = Nothing
        HasChanges = _friendRepository.HasChanges
        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
    End Sub

    Private Function OnRemovePhoneNumberCanExecute() As Boolean
        Return SelectedPhoneNumber IsNot Nothing
    End Function

    Public Overrides Async Sub OnSaveExecute()
        Await SaveWithOptimisticConcurrencyAsync(AddressOf _friendRepository.SaveAsync, Sub()
                                                                                            HasChanges = _friendRepository.HasChanges
                                                                                            Id = [Friend].Id
                                                                                            RaiseDetailSavedEvent(Id, [Friend].FirstName & " " & [Friend].LastName)
                                                                                        End Sub)
    End Sub

    Public Overrides Async Sub OnDeleteExecute()
        If Await _friendRepository.HasMeetingsAsync([Friend].Id) Then
            _messageDialogService.ShowInfoDialog("Warning", "Cannot delete because this friend is linked to a meeting")
            Return
        End If

        Dim result = Await _messageDialogService.ShowOkCancelDialog("Question", "Are you sure you wish to delete?")
        If result = MessageDialogResult.Ok Then
            _friendRepository.Remove([Friend].Model)
            Await _friendRepository.SaveAsync
            RaiseDetailDeletedEvent([Friend].Id)
        End If
    End Sub


    Public Overrides Function OnSaveCanExecute() As Boolean
        If [Friend] IsNot Nothing Then
            If PhoneNumbers.Any(Function(i) i.HasErrors = True) Then
                Return False
            End If
            Return Not [Friend].HasErrors And HasChanges
        Else
            Return True
        End If
    End Function

    Private Sub PhoneNumberWrapper_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If HasChanges = False Then
            HasChanges = _friendRepository.HasChanges
        End If
        If e.PropertyName = NameOf(PhoneNumberWrapper.HasErrors) Then
            DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
        End If
    End Sub

    Public Sub SetTitle()
        Title = [Friend].FirstName & " " & [Friend].LastName
    End Sub
End Class

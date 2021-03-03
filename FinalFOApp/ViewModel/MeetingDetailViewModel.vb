Imports System.Collections.ObjectModel
Imports Model
Imports Prism.Commands
Imports Prism.Events

Public Class MeetingDetailViewModel
    Inherits DetailViewModelBase
    Implements IMeetingDetailViewModel


    Private _meetingRepository As IMeetingRepository
    Private _meeting As MeetingWrapper
    Private _selectedAddedFriend As [Friend]
    Private _selectedAvailableFriend As [Friend]
    Private _allFriends As List(Of [Friend])

    Public Property AddedFriends As New ObservableCollection(Of [Friend])
    Public Property AvailableFriends As New ObservableCollection(Of [Friend])

    Public Property AddFriendCommand As ICommand
    Public Property RemoveFriendCommand As ICommand

    Public Property Meeting As MeetingWrapper
        Get
            Return _meeting
        End Get
        Set(value As MeetingWrapper)
            _meeting = value
            OnPropertyChanged()
        End Set
    End Property
    Public Property SelectedAddedFriend As [Friend]
        Get
            Return _selectedAddedFriend
        End Get
        Set(value As [Friend])
            _selectedAddedFriend = value
            OnPropertyChanged()
            DirectCast(RemoveFriendCommand, DelegateCommand).RaiseCanExecuteChanged()
        End Set
    End Property
    Public Property SelectedAvailableFriend As [Friend]
        Get
            Return _selectedAvailableFriend
        End Get
        Set(value As [Friend])
            _selectedAvailableFriend = value
            OnPropertyChanged()
            DirectCast(AddFriendCommand, DelegateCommand).RaiseCanExecuteChanged()
        End Set
    End Property

    Sub New(eventAggregator As IEventAggregator,
            messageDialogService As IMessageDialogService,
            meetingRepo As IMeetingRepository)
        MyBase.New(eventAggregator, messageDialogService)
        Me._eventAggregator = eventAggregator
        Me._messageDialogService = messageDialogService
        Me._meetingRepository = meetingRepo

        eventAggregator.GetEvent(Of AfterDetailSavedEvent).Subscribe(AddressOf AfterDetailSaved)
        eventAggregator.GetEvent(Of AfterDetailDeletedEvent).Subscribe(AddressOf AfterDetailedDeleted)

        AddFriendCommand = New DelegateCommand(AddressOf OnAddFriendExecute, AddressOf OnAddFriendCanExecute)
        RemoveFriendCommand = New DelegateCommand(AddressOf OnRemoveFriendExecute, AddressOf OnRemoveFriendCanExecute)

    End Sub

    Private Async Sub AfterDetailedDeleted(args As AfterDetailDeletedEventArgs)
        If args.ViewModelName = NameOf(FriendDetailViewModel) Then
            _allFriends = Await _meetingRepository.GetAllFriendsAsync
            SetupPickList()
        End If
    End Sub

    Private Async Sub AfterDetailSaved(args As AfterDetailSavedEventArgs)
        If args.ViewModelName = NameOf(FriendDetailViewModel) Then
            Await _meetingRepository.ReloadFriendAsync(args.Id)
            _allFriends = Await _meetingRepository.GetAllFriendsAsync
            SetupPickList()
        End If
    End Sub

    Public Overrides Async Function LoadAsync(id As Integer) As Task
        Dim meeting As Meeting
        If id > 0 Then
            meeting = Await _meetingRepository.GetByIdAsync(id)
        Else
            meeting = CreateNewMeeting()
        End If
        InitializeMeeting(meeting)

        Me.Id = meeting.Id

        _allFriends = Await _meetingRepository.GetAllFriendsAsync
        SetupPickList()
    End Function

    Private Sub SetupPickList()
        Dim MeetingFriendsIds = Me.Meeting.Model.Friends.Select(Function(i) i.Id).ToList
        Dim tempAddedFriends = _allFriends.Where(Function(i) MeetingFriendsIds.Contains(i.Id))
        Dim tempAvailableFriends = _allFriends.Except(tempAddedFriends)

        AddedFriends.Clear()
        AvailableFriends.Clear()

        For Each frend In tempAddedFriends
            AddedFriends.Add(frend)
        Next

        For Each frend In tempAvailableFriends
            AvailableFriends.Add(frend)
        Next
    End Sub

    Private Sub InitializeMeeting(meeting As Meeting)
        Me.Meeting = New MeetingWrapper(meeting)
        AddHandler Me.Meeting.PropertyChanged, Sub(s, e)
                                                   If HasChanges = False Then
                                                       HasChanges = _meetingRepository.HasChanges
                                                   End If
                                                   If e.PropertyName = NameOf(MeetingWrapper.HasErrors) Then
                                                       DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
                                                   End If
                                                   If e.PropertyName = NameOf(meeting.Title) Then
                                                       SetTitle()
                                                   End If
                                               End Sub
        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
        If Me.Meeting.Id = 0 Then
            Me.Meeting.Title = ""
        End If
        SetTitle()
    End Sub

    Private Function CreateNewMeeting() As Meeting
        Dim newMeeting As New Meeting With {
            .DateFrom = Now,
            .DateTo = Now}

        _meetingRepository.Add(newMeeting)
        Return newMeeting
    End Function

    Private Sub OnRemoveFriendExecute()
        Dim friendToRemove = SelectedAddedFriend
        Meeting.Model.Friends.Remove(friendToRemove)
        AvailableFriends.Add(friendToRemove)
        AddedFriends.Remove(friendToRemove)
        HasChanges = _meetingRepository.HasChanges
        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
    End Sub

    Private Function OnRemoveFriendCanExecute() As Boolean
        Return SelectedAddedFriend IsNot Nothing
    End Function

    Private Sub OnAddFriendExecute()
        Dim friendToAdd = SelectedAvailableFriend
        Meeting.Model.Friends.Add(friendToAdd)
        AddedFriends.Add(friendToAdd)
        AvailableFriends.Remove(friendToAdd)
        HasChanges = _meetingRepository.HasChanges
        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
    End Sub

    Private Function OnAddFriendCanExecute() As Boolean
        Return SelectedAvailableFriend IsNot Nothing
    End Function

    Public Overrides Async Sub OnSaveExecute()
        Await _meetingRepository.SaveAsync
        HasChanges = _meetingRepository.HasChanges
        Id = Meeting.Id
        RaiseDetailSavedEvent(Meeting.Id, Meeting.Title)
    End Sub

    Public Overrides Async Sub OnDeleteExecute()
        Dim result = Await Me._messageDialogService.ShowOkCancelDialog("Question", "Are you sure you wish to delete?")
        If result = MessageDialogResult.Ok Then
            _meetingRepository.Remove(Meeting.Model)
            Await _meetingRepository.SaveAsync
            RaiseDetailDeletedEvent(Meeting.Model.Id)
        End If
    End Sub

    Public Overrides Function OnSaveCanExecute() As Boolean
        Return Meeting IsNot Nothing AndAlso Meeting.HasErrors = False And HasChanges = True
    End Function

    Private Sub SetTitle()
        Title = Meeting.Title
    End Sub
End Class

Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Model
Imports Prism.Commands
Imports Prism.Events

Public Class ProgrammingLanguageDetailViewModel
    Inherits DetailViewModelBase

    Private _programmingLanguageRepository As IProgrammingLanguageRepository
    Private _selectedProgrammingLanguage As ProgrammingLanguageWrapper

    Public Property ProgrammingLanguages As New ObservableCollection(Of ProgrammingLanguageWrapper)
    Public Property SelectedProgrammingLanguage As ProgrammingLanguageWrapper
        Get
            Return _selectedProgrammingLanguage
        End Get
        Set(value As ProgrammingLanguageWrapper)
            _selectedProgrammingLanguage = value
            OnPropertyChanged()
        End Set
    End Property
    Public Property AddCommand As ICommand
    Public Property RemoveCommand As ICommand

    Sub New(eventAggregator As IEventAggregator, messageDialogService As IMessageDialogService, programmingLanguageRepository As IProgrammingLanguageRepository)
        MyBase.New(eventAggregator, messageDialogService)
        _programmingLanguageRepository = programmingLanguageRepository
        Title = "Programming Langauges"

        AddCommand = New DelegateCommand(AddressOf OnAddExecute)
        RemoveCommand = New DelegateCommand(AddressOf OnRemoveExecute, AddressOf OnRemoveCanExecute)
    End Sub

    Public Overrides Async Function LoadAsync(id As Integer) As Task
        Me.Id = id

        For Each wrapper In ProgrammingLanguages
            RemoveHandler wrapper.PropertyChanged, AddressOf Wrapper_PropertyChanged
        Next
        ProgrammingLanguages.Clear()
        Dim languages = Await _programmingLanguageRepository.GetAllAsync()
        For Each lang In languages
            Dim wrapper As New ProgrammingLanguageWrapper(lang)
            AddHandler wrapper.PropertyChanged, AddressOf Wrapper_PropertyChanged
            ProgrammingLanguages.Add(wrapper)
        Next
    End Function

    Public Sub Wrapper_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If HasChanges = False Then
            HasChanges = _programmingLanguageRepository.HasChanges
        End If
        If e.PropertyName = NameOf(ProgrammingLanguageWrapper.HasErrors) Then
            DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
        End If
    End Sub

    Public Overrides Async Sub OnSaveExecute()
        Await _programmingLanguageRepository.SaveAsync()
        HasChanges = _programmingLanguageRepository.HasChanges
        RaiseCollectionSavedEvent()
    End Sub

    Public Overrides Sub OnDeleteExecute()
        Throw New NotImplementedException()
    End Sub

    Public Async Sub OnRemoveExecute()
        Dim isReferenced = Await _programmingLanguageRepository.IsReferencedByFriendAsync(SelectedProgrammingLanguage.Id)
        If isReferenced Then
            _messageDialogService.ShowInfoDialog("Warning", "Cannot remove as this language is currently referenced by a friend")
            Return
        End If

        RemoveHandler SelectedProgrammingLanguage.PropertyChanged, AddressOf Wrapper_PropertyChanged
        _programmingLanguageRepository.Remove(SelectedProgrammingLanguage.Model)
        ProgrammingLanguages.Remove(SelectedProgrammingLanguage)
        SelectedProgrammingLanguage = Nothing
        HasChanges = _programmingLanguageRepository.HasChanges
        DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
    End Sub

    Public Function OnRemoveCanExecute() As Boolean
        Return SelectedProgrammingLanguage IsNot Nothing
    End Function

    Public Overrides Function OnSaveCanExecute() As Boolean
        Return HasChanges AndAlso ProgrammingLanguages.All(Function(i) i.HasErrors = False)
    End Function

    Private Sub OnAddExecute()
        Dim wrapper = New ProgrammingLanguageWrapper(New ProgrammingLanguage)
        AddHandler wrapper.PropertyChanged, AddressOf Wrapper_PropertyChanged
        _programmingLanguageRepository.Add(wrapper.Model)
        ProgrammingLanguages.Add(wrapper)

        wrapper.Name = ""
    End Sub
End Class

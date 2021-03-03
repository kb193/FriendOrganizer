Imports System.Data.Entity.Infrastructure
Imports Prism.Commands
Imports Prism.Events

Public MustInherit Class DetailViewModelBase
    Inherits ViewModelBase
    Implements IDetailViewModel

    Private _hasChanges As Boolean
    Private _title As String
    Public _eventAggregator As IEventAggregator
    Public _messageDialogService As IMessageDialogService


    Public Property SaveCommand As ICommand
    Public Property DeleteCommand As ICommand
    Public Property CloseDetailViewCommand As ICommand
    Public Property Id As Integer Implements IDetailViewModel.Id
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
            OnPropertyChanged()
        End Set
    End Property
    Public Property HasChanges As Boolean Implements IDetailViewModel.HasChanges
        Get
            Return _hasChanges
        End Get
        Set(value As Boolean)
            _hasChanges = value
            OnPropertyChanged()
            DirectCast(SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
        End Set
    End Property



    Sub New(eventAggregator As IEventAggregator, messageDialogService As IMessageDialogService)
        Me._eventAggregator = eventAggregator
        Me._messageDialogService = messageDialogService

        SaveCommand = New DelegateCommand(AddressOf OnSaveExecute, AddressOf OnSaveCanExecute)
        DeleteCommand = New DelegateCommand(AddressOf OnDeleteExecute)
        CloseDetailViewCommand = New DelegateCommand(AddressOf OnCloseDetailViewExecute)
    End Sub


    Public MustOverride Async Function LoadAsync(id As Integer) As Task Implements IDetailViewModel.LoadAsync
    Public MustOverride Function OnSaveCanExecute() As Boolean
    Public MustOverride Sub OnSaveExecute()
    Public MustOverride Sub OnDeleteExecute()
    Public Overridable Async Sub OnCloseDetailViewExecute()
        If HasChanges Then
            Dim result = Await _messageDialogService.ShowOkCancelDialog("You've made changes, close this item?", "Quetion")
            If result = MessageDialogResult.Cancel Then
                Return
            End If
        End If
        _eventAggregator.GetEvent(Of AfterDetailClosedEvent).Publish(New AfterDetailClosedEventArgs With {
                                                                    .Id = Me.Id,
                                                                    .ViewModelName = Me.GetType.Name})
    End Sub

    Public Sub RaiseDetailDeletedEvent(modelId As Integer)
        _eventAggregator.GetEvent(Of AfterDetailDeletedEvent).Publish(New AfterDetailDeletedEventArgs With {
                                                                      .Id = modelId, .ViewModelName = Me.GetType.Name})
    End Sub

    Public Sub RaiseDetailSavedEvent(modelId As Integer, displayMember As String)
        _eventAggregator.GetEvent(Of AfterDetailSavedEvent).Publish(New AfterDetailSavedEventArgs With {
                                                                    .Id = modelId,
                                                                    .ViewModelName = Me.GetType.Name,
                                                                    .DisplayMember = displayMember})
    End Sub

    Public Sub RaiseCollectionSavedEvent()
        _eventAggregator.GetEvent(Of AfterCollectionSavedEvent).Publish(New AfterCollectionSavedEventArgs With {
                                                                       .ViewModelName = Me.GetType.Name})
    End Sub

    Public Async Function SaveWithOptimisticConcurrencyAsync(saveFunc As Func(Of Task), afterSaveFunc As Action) As Task
        Dim entityDeleted As Boolean
        Dim entityChangedByAnotherUser As Boolean
        Dim entity As DbEntityEntry = Nothing
        Try
            Await saveFunc()
        Catch ex As DbUpdateConcurrencyException
            Dim dbValues = ex.Entries.Single.GetDatabaseValues
            If dbValues Is Nothing Then
                entityDeleted = True
            Else
                entityChangedByAnotherUser = True
                entity = ex.Entries.Single
            End If
        End Try

        If entityDeleted Then
            _messageDialogService.ShowInfoDialog("Warning", "Entity has been deleted")
            RaiseDetailDeletedEvent(Me.Id)
            Return
        End If

        If entityChangedByAnotherUser Then
            Dim result = Await _messageDialogService.ShowOkCancelDialog("Warning", "Entity has been changed by another user. Press OK to save your changes, or Cancel to reload entity from Database")
            If result = MessageDialogResult.Ok Then
                If entity IsNot Nothing Then
                    entity.OriginalValues.SetValues(entity.GetDatabaseValues)
                    Await saveFunc()
                End If
            Else
                Await entity.ReloadAsync
                Await LoadAsync(Me.Id)
            End If
        End If

        afterSaveFunc()
    End Function

End Class



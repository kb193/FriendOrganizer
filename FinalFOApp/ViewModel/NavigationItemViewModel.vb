Imports Prism.Commands
Imports Prism.Events

Public Class NavigationItemViewModel
    Inherits ViewModelBase

    Private _displayMember As String
    Private _detailViewModelName As String
    Private _eventAggregator As IEventAggregator

    Public Property OpenDetailViewCommand As ICommand

    Public Property Id As Integer
    Public Property DisplayMember As String
        Get
            Return _displayMember
        End Get
        Set(value As String)
            _displayMember = value
            OnPropertyChanged()
        End Set
    End Property


    Sub New(id As Integer, displayMember As String, eventAggregator As IEventAggregator, detailViewModelName As String)
        Me.Id = id
        Me.DisplayMember = displayMember
        Me._eventAggregator = eventAggregator
        Me._detailViewModelName = detailViewModelName

        OpenDetailViewCommand = New DelegateCommand(AddressOf OnOpenDetailViewExecute)
    End Sub

    Private Sub OnOpenDetailViewExecute()
        _eventAggregator.GetEvent(Of OpenDetailViewEvent).Publish(New OpenDetailViewEventArgs With {
                                                                  .Id = Id,
                                                                  .ViewModelName = _detailViewModelName})
    End Sub
End Class

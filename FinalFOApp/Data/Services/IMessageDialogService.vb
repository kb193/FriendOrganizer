Public Interface IMessageDialogService
    Sub ShowInfoDialog(title As String, message As String)
    Function ShowOkCancelDialog(title As String, message As String) As Task(Of MessageDialogResult)
End Interface

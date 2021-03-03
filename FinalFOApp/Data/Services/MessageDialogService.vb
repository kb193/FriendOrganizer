Imports MahApps.Metro.Controls
Imports MahApps.Metro.Controls.Dialogs

Public Class MessageDialogService
    Implements IMessageDialogService

    Public ReadOnly Property MetroWindow As MetroWindow
        Get
            Return DirectCast(Application.Current.MainWindow, MetroWindow)
        End Get
    End Property

    Public Async Function ShowOkCancelDialog(title As String, message As String) As Task(Of MessageDialogResult) Implements IMessageDialogService.ShowOkCancelDialog
        Dim result = Await MetroWindow.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative)

        If result = Dialogs.MessageDialogResult.Affirmative Then
            Return MessageDialogResult.Ok
        Else
            Return MessageDialogResult.Cancel
        End If
    End Function

    Public Sub ShowInfoDialog(title As String, message As String) Implements IMessageDialogService.ShowInfoDialog
        MetroWindow.ShowMessageAsync(title, message)
    End Sub
End Class

Public Enum MessageDialogResult
    Ok
    Cancel
End Enum

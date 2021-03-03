Imports Prism.Events

Public Class OpenDetailViewEvent
    Inherits PubSubEvent(Of OpenDetailViewEventArgs)
End Class
Public Class OpenDetailViewEventArgs
    Public Property Id As Integer
    Public Property ViewModelName As String
End Class

Imports Prism.Events

Public Class AfterDetailDeletedEvent
    Inherits PubSubEvent(Of AfterDetailDeletedEventArgs)
End Class
Public Class AfterDetailDeletedEventArgs
    Public Property Id As Integer
    Public Property ViewModelName As String
End Class

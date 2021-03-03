Imports Prism.Events

Public Class AfterDetailClosedEvent
    Inherits PubSubEvent(Of AfterDetailClosedEventArgs)
End Class
Public Class AfterDetailClosedEventArgs
    Public Property Id As Integer
    Public Property ViewModelName As String
End Class

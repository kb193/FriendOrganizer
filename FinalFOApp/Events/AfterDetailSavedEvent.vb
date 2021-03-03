Imports Prism.Events

Public Class AfterDetailSavedEvent
    Inherits PubSubEvent(Of AfterDetailSavedEventArgs)
End Class
Public Class AfterDetailSavedEventArgs
    Public Property Id As Integer
    Public Property ViewModelName As String
    Public Property DisplayMember
End Class

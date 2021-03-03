Imports Prism.Events

Public Class AfterCollectionSavedEvent
    Inherits PubSubEvent(Of AfterCollectionSavedEventArgs)
End Class
Public Class AfterCollectionSavedEventArgs
    Public Property ViewModelName As String
End Class

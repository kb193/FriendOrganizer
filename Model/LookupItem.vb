Public Class LookupItem

    Public Property Id As Integer

    Public Property DisplayMember As String

End Class
Public Class NullLookupItem
    Inherits LookupItem

    Public Overloads ReadOnly Property Id As Integer?
        Get
            Return Nothing
        End Get
    End Property
End Class

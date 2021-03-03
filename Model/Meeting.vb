Imports System.Collections.ObjectModel

Public Class Meeting

    Public Property Id As Integer

    Public Property Title As String

    Public Property DateFrom As Date

    Public Property DateTo As Date

    Public Property Friends As ICollection(Of [Friend])

    Sub New()
        Friends = New Collection(Of [Friend])
    End Sub

End Class

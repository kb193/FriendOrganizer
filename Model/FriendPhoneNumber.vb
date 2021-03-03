Imports System.ComponentModel.DataAnnotations

Public Class FriendPhoneNumber

    Public Property Id As Integer
    <Required>
    <Phone>
    Public Property Number As String

    Public Property FriendId As Integer

    Public Property [Friend] As [Friend]

End Class

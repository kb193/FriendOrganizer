Imports System.Collections.ObjectModel
Imports System.ComponentModel.DataAnnotations

Public Class [Friend]

    Sub New()
        Meetings = New Collection(Of Meeting)
        PhoneNumbers = New Collection(Of FriendPhoneNumber)
    End Sub

    Public Property Id As Integer

    <Required>
    <MaxLength(50)>
    Public Property FirstName As String

    <MaxLength(50)>
    Public Property LastName As String

    <EmailAddress>
    Public Property Email As String

    Public Property FavouriteLanguageId As Integer?

    Public Property FavouriteLanguage As ProgrammingLanguage

    <Timestamp>
    Public Property RowVersion As Byte()

    Public Property PhoneNumbers As ICollection(Of FriendPhoneNumber)

    Public Property Meetings As ICollection(Of Meeting)

End Class

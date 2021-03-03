Imports Model

Public Class MeetingWrapper
    Inherits ModelWrapper(Of Meeting)

    Public Sub New(model As Meeting)
        MyBase.New(model)
    End Sub

    Public ReadOnly Property Id As Integer
        Get
            Return Model.Id
        End Get
    End Property
    Public Property Title As String
        Get
            Return GetValue(Of String)()
        End Get
        Set(value As String)
            SetValue(value)
        End Set
    End Property

    Public Property DateFrom As Date
        Get
            Return GetValue(Of Date)()
        End Get
        Set(value As Date)
            SetValue(value)
        End Set
    End Property

    Public Property DateTo As Date
        Get
            Return GetValue(Of Date)()
        End Get
        Set(value As Date)
            SetValue(value)
        End Set
    End Property
End Class

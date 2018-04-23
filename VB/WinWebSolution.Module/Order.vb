Imports System
Imports DevExpress.Xpo
Imports DevExpress.Persistent.BaseImpl

Namespace WinWebSolution.Module
    Public Class Order
        Inherits BaseObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Private fDescription As String
        Public Property Description() As String
            Get
                Return fDescription
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Description", fDescription, value)
            End Set
        End Property
        Private fTotal As Decimal
        Public Property Total() As Decimal
            Get
                Return fTotal
            End Get
            Set(ByVal value As Decimal)
                SetPropertyValue("Total", fTotal, value)
                If (Not IsLoading) AndAlso (Not IsSaving) AndAlso Product IsNot Nothing Then
                    Product.UpdateOrdersTotal(True)
                    Product.UpdateMaximumOrder(True)
                End If
            End Set
        End Property
        Private fProduct As Product
        <Association("Product-Orders")> _
        Public Property Product() As Product
            Get
                Return fProduct
            End Get
            Set(ByVal value As Product)
                Dim oldProduct As Product = fProduct
                SetPropertyValue("Product", fProduct, value)
                If (Not IsLoading) AndAlso (Not IsSaving) AndAlso oldProduct IsNot fProduct Then
                    oldProduct = If(oldProduct, fProduct)
                    oldProduct.UpdateOrdersCount(True)
                    oldProduct.UpdateOrdersTotal(True)
                    oldProduct.UpdateMaximumOrder(True)
                End If
            End Set
        End Property
    End Class
End Namespace
Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Updating

Namespace WinWebSolution.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Using uow As New UnitOfWork(Session.DataLayer)
				Dim product As New Product(uow)
				product.Name = "Chai"
				For i As Integer = 0 To 9
					Dim order As New Order(uow)
					order.Product = product
					order.Description = "Order " & i.ToString()
					order.Total = i
				Next i
				uow.CommitChanges()
			End Using
		End Sub
	End Class
End Namespace
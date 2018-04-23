Imports System
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl

Namespace WinWebSolution.Module
	<DefaultClassOptions> _
	Public Class Product
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private fName As String
		Public Property Name() As String
			Get
				Return fName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", fName, value)
			End Set
		End Property
		<Association("Product-Orders"), Aggregated> _
		Public ReadOnly Property Orders() As XPCollection(Of Order)
			Get
				Return GetCollection(Of Order)("Orders")
			End Get
		End Property
		'You can use ready functions to perform aggregate operations on your collections:  Avg, Count, Exists, Max, Min, Max.
		'Please refer to the "Criteria Language Syntax" help topic (http://www.devexpress.com/Help/?document=XPO/CustomDocument4928.htm) for more information.
		'Note that this variant is shorter than when using the PersistentAliasAttribute.
		'Note that use of non-persistent calculated properties implemented via the PersistentAliasAttribute and Evaluate/EvaluateAlias methods
		'may be inappropriate in certain scenarios, especially when a large number of objects must be manipulated. This is because
		'each time such a property is accessed, a query to the database must be generated to evaluate the property for each master object.
		Private fOrdersCount As Nullable(Of Integer) = Nothing
		Public ReadOnly Property OrdersCount() As Nullable(Of Integer)
			Get
				If Not fOrdersCount.HasValue Then
					UpdateOrdersCount(False)
				End If
				Return fOrdersCount
			End Get
		End Property
		<Persistent("OrdersTotal")> _
		Private fOrdersTotal As Nullable(Of Decimal) = Nothing
		<PersistentAlias("fOrdersTotal")> _
		Public ReadOnly Property OrdersTotal() As Nullable(Of Decimal)
			Get
				If Not fOrdersTotal.HasValue Then
					UpdateOrdersTotal(False)
				End If
				Return fOrdersTotal
			End Get
		End Property
		Private fMaximumOrder As Nullable(Of Decimal) = Nothing
		Public ReadOnly Property MaximumOrder() As Nullable(Of Decimal)
			Get
				If Not fMaximumOrder.HasValue Then
					UpdateMaximumOrder(False)
				End If
				Return fMaximumOrder
			End Get
		End Property
		Protected Overrides Sub OnLoaded()
			MyBase.OnLoaded()
			'When using "lazy" calculations it's necessary to reset cached values.
			Reset()
		End Sub
		Private Sub Reset()
			fOrdersCount = Nothing
			fOrdersTotal = Nothing
			fMaximumOrder = Nothing
		End Sub
		'Define a way to calculate and update the OrdersCount;
		Public Sub UpdateOrdersCount(ByVal forceChangeEvents As Boolean)
			Dim oldOrdersCount As Nullable(Of Integer) = fOrdersCount
			'This line always evaluates the given expression on the server side so it doesn't take into account uncommitted objects.
			'fOrdersCount = Convert.ToInt32(Session.Evaluate<Product>(CriteriaOperator.Parse("Orders.Count"), CriteriaOperator.Parse("Oid=?", Oid)));
			'This line always evaluates the given expression on the client side using the objects loaded from an internal XPO cache.
			fOrdersCount = Convert.ToInt32(Evaluate(CriteriaOperator.Parse("Orders.Count")))

			If forceChangeEvents Then
				OnChanged("OrdersCount", oldOrdersCount, fOrdersCount)
			End If
		End Sub
		'Define a way to calculate and update the OrdersTotal;
		Public Sub UpdateOrdersTotal(ByVal forceChangeEvents As Boolean)
			'Put your complex business logic here. Just for demo purposes, we calculate a sum here.
			Dim oldOrdersTotal As Nullable(Of Decimal) = fOrdersTotal
			Dim tempTotal As Decimal = 0D
			'Manually iterate through the Orders collection if your calculated property requires a complex business logic which cannot be expressed via criteria language.
			For Each detail As Order In Orders
				tempTotal += detail.Total
			Next detail
			fOrdersTotal = tempTotal
			If forceChangeEvents Then
				OnChanged("OrdersTotal", oldOrdersTotal, fOrdersTotal)
			End If
		End Sub
		'Define a way you will calculate and update the MaximumOrder;
		Public Sub UpdateMaximumOrder(ByVal forceChangeEvents As Boolean)
			Dim oldMaximumOrder As Nullable(Of Decimal) = fMaximumOrder
			Dim tempMaximum As Decimal = 0D
			'Manually iterate through the Orders collection if your calculated property requires a complex business logic, which cannot be expressed via criteria language.
			For Each detail As Order In Orders
				'Put your complex business logic here. Just for demo purposes, we calculate a maximum here.
				If detail.Total > tempMaximum Then
					tempMaximum = detail.Total
				End If
			Next detail
			fMaximumOrder = tempMaximum
			If forceChangeEvents Then
				OnChanged("MaximumOrder", oldMaximumOrder, fMaximumOrder)
			End If
		End Sub
	End Class
End Namespace
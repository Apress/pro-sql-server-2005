Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient

<WebService(Namespace:="http://AdventureWorks.org")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
Public Class Service
    Inherits System.Web.Services.WebService
    Dim DBCon As New SqlConnection("Server=localhost;Database=AdventureWorks; integrated security=SSPI;")


    Public Class Category
        Public Name As String
        Public SubCategories As SubCategory()
    End Class

    Public Class SubCategory
        Public Name As String
        Public Products As Product()
    End Class
    Public Class Product
        Public ID As Integer
        Public Name As String
        Public Number As String
        Public Color As String
        Public Cost As Double
        Public Price As Double
        Public Size As String
        Public Weight As String

    End Class
    Public Sub Service()

    End Sub

    <WebMethod()> _
    Public Function GetProducts() As Category()
        Dim DBCmd As New SqlCommand()
        Dim SQLRead As SqlDataReader
        Dim Retval As Category()
        Dim Cat As Category
        Dim Prod As Product
        Dim SubCat As SubCategory
        Dim CatArray As New ArrayList
        Dim SubCatArray As New ArrayList
        Dim ProdArray As New ArrayList

        DBCon.Open()
        DBCmd.Connection = DBCon
        DBCmd.CommandText = "exec GetProducts"
        SQLRead = DBCmd.ExecuteReader()

        Cat = New Category
        SubCat = New SubCategory



        While SQLRead.Read

            If SQLRead("CategoryName").ToString <> Cat.Name Then
                If SubCatArray.Count <> 0 Then
                    Cat.SubCategories = SubCatArray.ToArray(SubCat.GetType)
                    SubCatArray.Clear()
                End If
                Cat = New Category
                Cat.Name = SQLRead("CategoryName").ToString
                CatArray.Add(Cat)
            End If
            If SQLRead("SubCategoryName").ToString <> SubCat.Name Then
                If ProdArray.Count <> 0 Then
                    SubCat.Products = ProdArray.ToArray(Prod.GetType)
                    ProdArray.Clear()
                End If
                SubCat = New SubCategory
                SubCat.Name = SQLRead("SubCategoryName").ToString
                SubCatArray.Add(SubCat)
                'Cat.SubCategories.Add(SubCat)
            End If

            Prod = New Product
            'SubCat.Products.Products.Add(Prod)
            ProdArray.Add(Prod)
            Prod.ID = CInt(SQLRead("ProductID").ToString)
            Prod.Name = SQLRead("Name").ToString
            Prod.Color = IIf(SQLRead("Color") Is DBNull.Value, "", SQLRead("Color")).ToString
            Prod.Cost = CDbl(SQLRead("StandardCost").ToString)
            Prod.Number = SQLRead("ProductNumber").ToString
            Prod.Price = CDbl(SQLRead("ListPrice").ToString)
            Prod.Size = IIf(SQLRead("Size") Is DBNull.Value, "", SQLRead("Size")).ToString
            Prod.Weight = IIf(SQLRead("Weight") Is DBNull.Value, "", SQLRead("Weight")).ToString



        End While
        If ProdArray.Count <> 0 Then
            SubCat.Products = ProdArray.ToArray(Prod.GetType)
            ProdArray.Clear()
        End If
        If SubCatArray.Count <> 0 Then
            Cat.SubCategories = SubCatArray.ToArray(SubCat.GetType)
            SubCatArray.Clear()
        End If
        DBCon.Close()
        Return CatArray.ToArray(Cat.GetType)

    End Function

End Class
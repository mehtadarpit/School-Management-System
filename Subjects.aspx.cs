using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Subjects : System.Web.UI.Page
{
    private string constring = "Data Source=.;Initial Catalog=Client;Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) // Load data only on the first page load
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        using (SqlConnection Sqlcon = new SqlConnection(constring))
        {
            string query = "SELECT * FROM Product"; // Consider using parameterized queries if needed
            try
            {
                Sqlcon.Open();
                using (SqlDataAdapter sqlData = new SqlDataAdapter(query, Sqlcon))
                {
                    DataTable dataTable = new DataTable();
                    sqlData.Fill(dataTable);
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, show a message, etc.)
                // For example: Response.Write("Error: " + ex.Message);
            }
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace ContactManagement
{
    public partial class Contact : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ContactDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                lblUsername.Text = Session["Username"].ToString();
                if (!IsPostBack || Request.QueryString["updated"] == "true")
                {
                    LoadContacts();
                }
            }
        }

        protected void LoadContacts(string searchQuery = "")
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT ContactID, ContactName, ContactPhone FROM Contacts WHERE UserID = (SELECT UserID FROM Users WHERE Username = @Username)";
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " AND (ContactName LIKE @SearchQuery OR ContactPhone LIKE @SearchQuery)";
                }
                query += " ORDER BY ContactName";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", Session["Username"]);
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvContacts.DataSource = dt;
                gvContacts.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddContact.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchContainer.Visible = !searchContainer.Visible;
        }

        protected void btnPerformSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            LoadContacts(searchQuery);
        }

        protected void gvContacts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int contactId = Convert.ToInt32(e.CommandArgument);
                DeleteContact(contactId);
                Response.Redirect("Contact.aspx"); // Redirect to home page after deletion
            }
            else if (e.CommandName == "Edit")
            {
                int contactId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("EditContact.aspx?ContactID=" + contactId);
            }
        }

        protected void gvContacts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int contactId = Convert.ToInt32(gvContacts.DataKeys[e.RowIndex].Value);
            DeleteContact(contactId);
            LoadContacts();  
        }


        protected void DeleteContact(int contactId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Contacts WHERE ContactID = @ContactID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ContactID", contactId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}

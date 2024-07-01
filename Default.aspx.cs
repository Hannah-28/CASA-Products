using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Description;

namespace ProductCreation
{
    public partial class ProductCreation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear the success message hidden field on every page load
            if (!IsPostBack)
            {
                hdnSuccessMessage.Value = string.Empty;
            }
        }

        private void Create_GLs(int productId)
        {
            //string[] glNames = { "Interest Payable GL", "Interest Expense GL", "General Fee Income GL", "Maintenance Fee GL" };
            //string[] glCodes = { "GL1", "GL2", "GL3", "GL4" };
            //string[] glClasses = { "Liability", "Expense", "Income", "Income" };

            List<(string GLName, string GLClass, string GLCode)> GLs = new List<(string, string, string)>();
            
            if (InterestPayable.Checked)
            {
                GLs.Add(("Interest Payable", "Liability", "GL1"));
            }
            if (InterestExpense.Checked)
            {
                GLs.Add(("Interest Expense", "Expense", "GL2"));
            } 
            if (GeneralFeeIncome.Checked)
            {
                GLs.Add(("General Fee Income", "Income", "GL3"));
            }
            if (MaintenanceFee.Checked)
            {
                GLs.Add(("Maintenance Fee", "Income", "GL4"));
            }
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True"))
            {
                conn.Open();
                foreach (var(GLName, GLCLass, GLCode) in GLs)
                {
                    // Insert new product
                    string insertQuery = "INSERT INTO [dbo].[GLsTable] (GLName, GLCode, GLClass, ProductCode) " +
                                   "VALUES (@GLName, @GLCode, @GLClass, @ProductCode)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@GLName", GLName);
                        insertCmd.Parameters.AddWithValue("@GLCode", GLCode);
                        insertCmd.Parameters.AddWithValue("@GLClass", GLCLass);
                        insertCmd.Parameters.AddWithValue("@ProductCode", productId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string otherCategoryName = txtOtherCategory.Text.Trim();
            bool paysInterest = chkPaysInterest.Checked;
            bool applyFees = chkApplyFees.Checked;
            bool smsNotification = chkSMSNotification.Checked;
            bool channelServicesAccess = chkChannelServicesAccess.Checked;

            // Use the otherCategoryName if 'Others' is selected, otherwise use the selected category
            string finalCategoryName = category == "Others" ? otherCategoryName : category;


            if (string.IsNullOrWhiteSpace(productName))
            {
                // Display an error message when product name is empty
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('Product Name is required.');", true);
                return;
            }
            if (category == "Others" && string.IsNullOrWhiteSpace(otherCategoryName))
            {
                // Display an error message when other category name is empty
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('Please enter a name for the Others category.');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True"))
            {
                conn.Open();
                // Check for duplicate product name
                string checkQuery = "SELECT COUNT(*) FROM [dbo].[Table] WHERE ProductName = @ProductName";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ProductName", productName);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        // Display an error message when product name is duplicate
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('Product Name already exists. Please choose a different name.');", true);
                        return;
                    }
                }

                // Generate a unique product code and ensure its uniqueness
                string productCode = GenerateUniqueProductCode();

                // Insert new product
                string insertQuery = @"INSERT INTO [dbo].[Table] (ProductName, Category, PaysInterest, ApplyFees, SMSNotification, ChannelServicesAccess, ProductCode) OUTPUT INSERTED.ProductCode VALUES (@ProductName, @Category, @PaysInterest, @ApplyFees, @SMSNotification, @ChannelServicesAccess, @ProductCode)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@ProductName", productName);
                    insertCmd.Parameters.AddWithValue("@Category", finalCategoryName);
                    insertCmd.Parameters.AddWithValue("@PaysInterest", paysInterest);
                    insertCmd.Parameters.AddWithValue("@ApplyFees", applyFees);
                    insertCmd.Parameters.AddWithValue("@SMSNotification", smsNotification);
                    insertCmd.Parameters.AddWithValue("@ChannelServicesAccess", channelServicesAccess);
                    insertCmd.Parameters.AddWithValue("@ProductCode", productCode);
                    // insertCmd.ExecuteNonQuery();
                    int productId = (int)insertCmd.ExecuteScalar();
                    Create_GLs(productId);
                }
            }
            // Set the success message to the hidden field
            hdnSuccessMessage.Value = "Product created successfully!";

            // Refresh the page to display the alert and clear the form
            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('Product created successfully!');window.location = '" + Request.RawUrl + "';", true);
        }


        private string GenerateUniqueProductCode()
        {
            string productCode = string.Empty;
            bool isUnique;
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True"))
                do
                {
                    conn.Open();
                    productCode = GenerateRandomProductCode();
                    string checkCodeQuery = "SELECT COUNT(*) FROM [dbo].[Table] WHERE ProductCode = @ProductCode";
                    using (SqlCommand checkCodeCmd = new SqlCommand(checkCodeQuery, conn))
                    {
                        checkCodeCmd.Parameters.AddWithValue("@ProductCode", productCode);
                        int count = (int)checkCodeCmd.ExecuteScalar();
                        isUnique = count == 0;
                    }
                } while (!isUnique);

            return productCode;
        }

        private string GenerateRandomProductCode()
        {
            Random random = new Random();
            return random.Next(100, 999).ToString();
        }



    }
}

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.EOperations;
using DAL.Entities;
using System.Data;


public partial class EditEmployee : System.Web.UI.Page
{
    EEmployee emp = new EEmployee();
    EOperation empHandler = new EOperation();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Bind Roles to DropDown
            ERole.DataSource = empHandler.GetEmployeeRole();
            ERole.DataTextField = "Role_Name";
            ERole.DataBind();
            EditEmp.Style.Add("display", "none");

            #region DisableControls
            // Loop through all controls on the page/form and disable them
            foreach (Control ctrl in form2.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)(ctrl)).Enabled = false;
                    if (((TextBox)ctrl).ID == "EID")
                        ((TextBox)(ctrl)).Enabled = true; // Keep EID enabled
                }
                else if (ctrl is Label)
                {
                    ((Label)(ctrl)).Enabled = false;
                }
                else if (ctrl is DropDownList)
                {
                    ((DropDownList)(ctrl)).Enabled = false;
                }
                else if (ctrl is CheckBox)
                {
                    ((CheckBox)(ctrl)).Checked = false;
                }
                else if (ctrl is CheckBoxList)
                {
                    ((CheckBoxList)(ctrl)).ClearSelection();
                }
                else if (ctrl is RadioButton)
                {
                    ((RadioButton)(ctrl)).Checked = false;
                }
                else if (ctrl is RadioButtonList)
                {
                    ((RadioButtonList)(ctrl)).ClearSelection();
                }
            }
            #endregion
        }
    }

    protected void EditEmp_Click(object sender, EventArgs e)
    {
        try
        {
            // Collect data from the form
            emp.ID = Convert.ToInt32(EID.Text);
            emp.FNAME = EFname.Text;
            emp.LNAME = ELname.Text;
            emp.EMAIL = EEmail.Text;
            emp.PASSWORD = EPass.Text;
            emp.DOB = Convert.ToDateTime(EDOB.Text).Date;
            emp.TELEPHONE = ETel.Text;
            emp.MOBILENO = EMoblie.Text;
            emp.DOJ = Convert.ToDateTime(EDOJ.Text).Date;
            emp.STATUS = EStatus.Text;
            emp.GENDER = EGender.Text;
            emp.ROLE = int.Parse(empHandler.GetEmployeeRoleId(ERole.Text)); // Role ID from dropdown
            emp.SALARY = decimal.Parse(Esalary.Text);

            // Update employee record
            if (empHandler.UpdateEmployee(emp, emp.ID) > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", "alert('Employee Record Updated!')", true);
            }
        }
        catch (Exception ex)
        {
            // Handle errors
            Response.Write("<script>alert('" + ex.Message + "')</script>");
        }

        // Show the "Get Data" button again after updating
        gETdata.Style.Add("display", "block");

        #region ClearInputs
        // Clear all inputs after updating
        foreach (Control ctrl in form2.Controls)
        {
            if (ctrl is TextBox)
            {
                ((TextBox)(ctrl)).Text = string.Empty;
            }
            else if (ctrl is Label)
            {
                ((Label)(ctrl)).Text = string.Empty;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)(ctrl)).SelectedIndex = 0; // Reset dropdown to default value
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)(ctrl)).Checked = false;
            }
            else if (ctrl is CheckBoxList)
            {
                ((CheckBoxList)(ctrl)).ClearSelection();
            }
            else if (ctrl is RadioButton)
            {
                ((RadioButton)(ctrl)).Checked = false;
            }
            else if (ctrl is RadioButtonList)
            {
                ((RadioButtonList)(ctrl)).ClearSelection();
            }
        }
        #endregion
    }

    protected void gETdata_Click(object sender, EventArgs e)
    {
        EID.ReadOnly = true;
        DataSet ds = empHandler.GetEmployeeByID(int.Parse(EID.Text));

        if (ds.Tables[0].Rows.Count > 0)
        {
            // Populate the form with the employee data
            EID.Text = ds.Tables[0].Rows[0]["E_ID"].ToString();
            EFname.Text = ds.Tables[0].Rows[0]["E_Fname"].ToString();
            ELname.Text = ds.Tables[0].Rows[0]["E_Lname"].ToString();
            EEmail.Text = ds.Tables[0].Rows[0]["E_Email"].ToString();
            EPass.Text = ds.Tables[0].Rows[0]["E_Pass"].ToString();
            EDOB.Text = ds.Tables[0].Rows[0]["E_DOB"].ToString();
            ETel.Text = ds.Tables[0].Rows[0]["E_TelNo"].ToString();
            EMoblie.Text = ds.Tables[0].Rows[0]["E_MobileNo"].ToString();
            EDOJ.Text = ds.Tables[0].Rows[0]["E_DOJ"].ToString();
            EStatus.Text = ds.Tables[0].Rows[0]["E_Status"].ToString();
            EGender.Text = ds.Tables[0].Rows[0]["E_Gender"].ToString();

            // Set Role dropdown based on retrieved Role ID
            int Rid = int.Parse(ds.Tables[0].Rows[0]["E_RoleID"].ToString());
            ERole.Text = empHandler.GetEmployeeRoleName(Rid);
            Esalary.Text = ds.Tables[0].Rows[0]["E_Salary"].ToString();

            // Hide "Get Data" button, show "Edit" button and enable form controls
            gETdata.Style.Add("display", "none");
            EditEmp.Style.Add("display", "block");

            #region EnableControls
            // Loop through all controls and enable them for editing
            foreach (Control ctrl in form2.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)(ctrl)).Enabled = true;
                    if (((TextBox)ctrl).ID == "EID")
                        ((TextBox)(ctrl)).Enabled = true; // Keep EID enabled
                }
                else if (ctrl is Label)
                {
                    ((Label)(ctrl)).Enabled = true;
                }
                else if (ctrl is DropDownList)
                {
                    ((DropDownList)(ctrl)).Enabled = true;
                }
                else if (ctrl is CheckBox)
                {
                    ((CheckBox)(ctrl)).Checked = true;
                }
                else if (ctrl is CheckBoxList)
                {
                    ((CheckBoxList)(ctrl)).ClearSelection();
                }
                else if (ctrl is RadioButton)
                {
                    ((RadioButton)(ctrl)).Checked = true;
                }
                else if (ctrl is RadioButtonList)
                {
                    ((RadioButtonList)(ctrl)).ClearSelection();
                }
            }
            #endregion
        }
    }

    // Helper method to check if a string contains only digits
    bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }
}

namespace xnaDemo
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelSkillLevel = new System.Windows.Forms.Label();
            this.skillLevel = new System.Windows.Forms.ComboBox();
            this.optionNoCheat = new System.Windows.Forms.RadioButton();
            this.optionInvincible = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSkillLevel
            // 
            this.labelSkillLevel.AutoSize = true;
            this.labelSkillLevel.Location = new System.Drawing.Point(12, 9);
            this.labelSkillLevel.Name = "labelSkillLevel";
            this.labelSkillLevel.Size = new System.Drawing.Size(54, 13);
            this.labelSkillLevel.TabIndex = 0;
            this.labelSkillLevel.Text = "Skill level:";
            // 
            // skillLevel
            // 
            this.skillLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skillLevel.FormattingEnabled = true;
            this.skillLevel.Items.AddRange(new object[] {
            "Beginner",
            "Intermediate",
            "Advanced"});
            this.skillLevel.Location = new System.Drawing.Point(72, 6);
            this.skillLevel.Name = "skillLevel";
            this.skillLevel.Size = new System.Drawing.Size(121, 21);
            this.skillLevel.TabIndex = 1;
            // 
            // optionNoCheat
            // 
            this.optionNoCheat.AutoSize = true;
            this.optionNoCheat.Checked = true;
            this.optionNoCheat.Location = new System.Drawing.Point(15, 33);
            this.optionNoCheat.Name = "optionNoCheat";
            this.optionNoCheat.Size = new System.Drawing.Size(70, 17);
            this.optionNoCheat.TabIndex = 2;
            this.optionNoCheat.TabStop = true;
            this.optionNoCheat.Text = "No Cheat";
            this.optionNoCheat.UseVisualStyleBackColor = true;
            // 
            // optionInvincible
            // 
            this.optionInvincible.AutoSize = true;
            this.optionInvincible.Location = new System.Drawing.Point(15, 56);
            this.optionInvincible.Name = "optionInvincible";
            this.optionInvincible.Size = new System.Drawing.Size(70, 17);
            this.optionInvincible.TabIndex = 3;
            this.optionInvincible.Text = "Invincible";
            this.optionInvincible.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(10, 88);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(91, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(251, 123);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.optionInvincible);
            this.Controls.Add(this.optionNoCheat);
            this.Controls.Add(this.skillLevel);
            this.Controls.Add(this.labelSkillLevel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSkillLevel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.RadioButton optionNoCheat;
        public System.Windows.Forms.RadioButton optionInvincible;
        public System.Windows.Forms.ComboBox skillLevel;
    }
}
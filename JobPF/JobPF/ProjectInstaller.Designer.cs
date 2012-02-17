namespace JobPF
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceJobProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceJobInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceJobProcessInstaller
            // 
            this.serviceJobProcessInstaller.Password = null;
            this.serviceJobProcessInstaller.Username = null;
            // 
            // serviceJobInstaller
            // 
            this.serviceJobInstaller.ServiceName = "ServiceJob";
            this.serviceJobInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceJobProcessInstaller,
            this.serviceJobInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceJobProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceJobInstaller;
    }
}
using System;
using System.Net.Mail;

/// <summary>
/// Class contains miscellaneous functionality 
/// </summary>
public static class Utilities
{
	static Utilities()
	{
		//
		// TODO: Add constructor logic here
		//
	}

  // Generic method for sending emails
  public static void SendMail(string from, string to, string subject, string body)
  {
    // Configure mail client (may need additional
    // code for authenticated SMTP servers)
    SmtpClient mailClient = new SmtpClient(BalloonShopConfiguration.MailServer);
    // Create the mail message
    MailMessage mailMessage = new MailMessage(from, to, subject, body);
    // Send mail
    mailClient.Send(mailMessage);
  }

  // Send error log mail
  public static void SendErrorLogEmail(Exception ex)
  {
    string from = "BalloonShop Error Report";
    string to = BalloonShopConfiguration.ErrorLogEmail;
    string subject = "BalloonShop Error Generated at " + DateTime.Now.ToShortDateString();
    string body = ex.Message + "\n\n" + "Stack trace:\n" + ex.StackTrace.ToString();
    SendMail(from, to, subject, body);
  }
}

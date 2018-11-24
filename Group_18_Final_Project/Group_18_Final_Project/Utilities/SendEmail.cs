using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace Group_18_Final_Project.Utilities
{
    public class SendEmail
    {
        public IActionResult Contact(string toaddress, string recipient, string subject, string body)
        {
            ViewData["Message"] = "Your Contact page TODO.";
            //instantiate mimemessage
            var message = new MimeMessage;
            //from address
            message.From.Add(new MailboxAddress("Admin", "fa18group18@gmail.com"));
            //to address

            //subject

            //body


            return View();
        }
    }
}

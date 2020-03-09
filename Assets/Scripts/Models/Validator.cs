using Assets.Scripts.Common;
using Assets.Scripts.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Validator
    {
        public bool IsValid { get; set; }

        public Validator() => IsValid = true;

        public void Invalidate() => IsValid = false;
    }

    public static class ValidatorExtensions
    {
        public static bool Validate(this object target)
        {
            var validator = new Validator();
            var notificationName = Global.ValidateNotification(target.GetType());
            target.PostNotification(notificationName, validator);
            return validator.IsValid;
        }
    }
}

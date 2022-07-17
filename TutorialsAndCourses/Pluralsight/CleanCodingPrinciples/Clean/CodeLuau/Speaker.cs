using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeLuau
{
    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<Session> Sessions { get; set; }

        /// <summary>
        /// Register a speaker
        /// </summary>
        /// <returns>speakerID</returns>
        public RegisterResponse Register(IRepository repository)
        {
            var registerError = ValidateRegistration();
            if (registerError is not null) return new RegisterResponse(registerError);
            
            var speakerId = repository.SaveSpeaker(this);
            
            return new RegisterResponse(speakerId);
        }

        private RegisterError? ValidateRegistration()
        {
            var registerError = ValidateUserInput();
            if (registerError is not null)
            {
                return registerError;
            }

            var speakerMeetsStandards = AppearsExceptional() || HasNoObviousRedFlags();
            if (!speakerMeetsStandards)
            {
                registerError = RegisterError.SpeakerDoesNotMeetStandards;
                return registerError;
            }
            
            var atLeastOneSessionApproved = ApproveSessions();
            if (!atLeastOneSessionApproved)
            {
                registerError = RegisterError.NoSessionsApproved;
                return registerError;
            }

            return null;
        }

        private bool ApproveSessions()
        {
            foreach (var session in Sessions)
            {
                session.Approved = !SessionIsAboutOldTechnology(session);
            }

            return Sessions.Any(s => s.Approved);
        }

        private static bool SessionIsAboutOldTechnology(Session session)
        {
            var oldTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            return oldTechnologies.Any(oldTechnology => session.Title.Contains(oldTechnology) || session.Description.Contains(oldTechnology));
        }

        private bool HasNoObviousRedFlags()
        {
            if (Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9) return false;
            string emailDomain = Email.Split('@').Last();
            var outdatedDomains = new List<string>() { "aol.com", "prodigy.com", "compuserve.com" };
            if (outdatedDomains.Contains(emailDomain)) return false;
            return true;
        }

        private bool AppearsExceptional()
        {
            if (YearsOfExperience > 10) return true;
            if (HasBlog) return true;
            if (Certifications.Count() > 3) return true;
            var preferredEmployers = new List<string>()
                { "Pluralsight", "Microsoft", "Google" };
            if (preferredEmployers.Contains(Employer)) return true;
            return false;
        }

        private RegisterError? ValidateUserInput()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return RegisterError.FirstNameRequired;
            if (string.IsNullOrWhiteSpace(LastName)) return RegisterError.LastNameRequired;
            if (string.IsNullOrWhiteSpace(Email)) return RegisterError.EmailRequired;
            if (!Sessions.Any()) return RegisterError.NoSessionsProvided;
            return null;
        }
    }
}
using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityRegisterResponseModel {

	public RegisterResult Result { get; set; }
	public IEnumerable<string>? Errors { get; }

	public IdentityRegisterResponseModel (RegisterResult result, IEnumerable<string>? errors = null) {
		Result = result;
		Errors = errors;
	}

	public enum RegisterResult {

		Success,
		Failed,
		RequiresConfirmation

	}

}
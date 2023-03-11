using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers; 

public interface IHtmlTransformer {

	public bool Applies (TransformMode mode);

	public string Up (Website website, string content);

	public string Down (string content);

}
using Interpreter.Recognizers.Result;

namespace Interpreter.Recognizers.Rule
{
	public class EntityRule
	{
		readonly EntityCategory _category;

		public EntityRule(EntityCategory category) => _category = category;

		public EntityCategory Category
		{
			get { return _category; }
		}

		public string Subcategory { get; set; } = "*";
		public string Text { get; set; } = "*";

		public Func<Entity, IEnumerable<Parameter>> ExtractParams { get; set; } = (e) => Array.Empty<Parameter>();

		public bool Match(Entity entity)
		{
			if (NotMatch(Category.ToString(), entity.Category)) { return false; }
			if (Subcategory != "*" && NotMatch(Subcategory, entity.SubCategory)) { return false; }
			if (Text != "*" && NotMatch(Text, entity.Text)) { return false; }
			return true;
		}

		private static bool NotMatch(string a, string b) =>
			!string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);

	}

}

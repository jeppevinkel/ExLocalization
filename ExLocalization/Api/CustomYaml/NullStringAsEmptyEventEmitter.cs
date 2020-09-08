using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace ExLocalization.Api.CustomYaml
{
	public class NullStringsAsEmptyEventEmitter : ChainedEventEmitter
	{
		public NullStringsAsEmptyEventEmitter(IEventEmitter nextEmitter)
			: base(nextEmitter)
		{
		}

		public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			if (eventInfo.Source.Type == typeof(string) && eventInfo.Source.Value == null)
				emitter.Emit(new Scalar(string.Empty));
			else
				base.Emit(eventInfo, emitter);
		}
	}
}
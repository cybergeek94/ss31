﻿using Microsoft.Xna.Framework;
using SS31.Common.Service;
using SS31.Client.Network;
using SS31.Common.Network;

namespace SS31.Client
{
	// This is just the test state, until we get a  nice splash screen and main menu and whatnot going.
	public class GameState : State
	{
		public override bool ShouldSuspend { get { return false; } }

		public GameState()
		{

		}

		public override void Dispose(bool disposing)
		{
			if (!IsDisposed && disposing)
			{
				if (ServiceManager.HasService<NetManager>())
					ServiceManager.Resolve<NetManager>().OnMessageRecieved -= handleNetworkMessage;
			}

			base.Dispose(disposing);
		}

		void handleNetworkMessage(object sender, IncomingNetMessageArgs args)
		{

		}

		protected override void Initialize()
		{
			ServiceManager.Resolve<NetManager>().OnMessageRecieved += handleNetworkMessage;
			// ServiceManager.Resolve<NetManager>().Connect(new IPEntry("127.0.0.1:8100"));
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(GameTime gameTime)
		{

		}
	}
}
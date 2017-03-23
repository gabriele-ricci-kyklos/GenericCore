using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.StateMachine;
using GenericCore.StateMachine.EventArgs;

namespace GenericCore.Test.StateMachine
{
    [TestClass]
    public class StateMachineTests
    {
        public enum States
        {
            R,
            G,
            B
        }

        [TestMethod]
        public void BasicTest()
        {
            StateMachine<States> machine = new StateMachine<States>();

            machine.AddState(States.R);
            machine.AddState(States.G);
            machine.AddState(States.B);

            machine.AddTransition(States.R, States.G);
            machine.AddTransition(States.R, States.B);
            machine.AddTransition(States.G, States.R);
            machine.AddTransition(States.G, States.B);
            machine.AddTransition(States.B, States.R);
            machine.AddTransition(States.B, States.G);

            machine.AddEnterStateCallback(States.R, (state) => Assert.AreEqual(States.R, state.Name));
            machine.AddEnterStateCallback(States.G, (state) => Assert.AreEqual(States.G, state.Name));
            machine.AddEnterStateCallback(States.B, (state) => Assert.AreEqual(States.B, state.Name));

            machine.AddExitStateCallback(States.R, (state) => Assert.AreEqual(States.R, state.Name));
            machine.AddExitStateCallback(States.G, (state) => Assert.AreEqual(States.G, state.Name));
            machine.AddExitStateCallback(States.B, (state) => Assert.AreEqual(States.B, state.Name));

            machine.AddTransitionCallback(States.R, States.G, (from, to) => { Assert.AreEqual(States.R, from.Name); Assert.AreEqual(States.G, to.Name); });
            machine.AddTransitionCallback(States.R, States.B, (from, to) => { Assert.AreEqual(States.R, from.Name); Assert.AreEqual(States.B, to.Name); });
            machine.AddTransitionCallback(States.G, States.R, (from, to) => { Assert.AreEqual(States.G, from.Name); Assert.AreEqual(States.R, to.Name); });
            machine.AddTransitionCallback(States.G, States.B, (from, to) => { Assert.AreEqual(States.G, from.Name); Assert.AreEqual(States.B, to.Name); });
            machine.AddTransitionCallback(States.B, States.R, (from, to) => { Assert.AreEqual(States.B, from.Name); Assert.AreEqual(States.R, to.Name); });
            machine.AddTransitionCallback(States.B, States.G, (from, to) => { Assert.AreEqual(States.B, from.Name); Assert.AreEqual(States.G, to.Name); });

            machine.Transiting += new EventHandler<TransitingEventArgs<States>>((obj, e) => Assert.IsTrue((obj as StateMachine<States>).IsTransiting));
            machine.Transited += new EventHandler<TransitedEventArgs<States>>((obj, e) => Assert.IsFalse((obj as StateMachine<States>).IsTransiting));

            machine.GoToState(States.R);
            machine.GoToState(States.G);
            machine.GoToState(States.B);
        }

        [TestMethod]
        public void BasicTestWithFluentInterface()
        {
            StateMachineBuilder<States> builder =
                StateMachineBuilder<States>
                    .New()
                    .State(States.R)
                    .State(States.G)
                    .State(States.B)

                    .Transition(States.R, States.G)
                    .Transition(States.R, States.B)
                    .Transition(States.G, States.R)
                    .Transition(States.G, States.B)
                    .Transition(States.B, States.R)
                    .Transition(States.B, States.G)

                    .Entering(States.R, (state) => Assert.AreEqual(States.R, state.Name))
                    .Entering(States.G, (state) => Assert.AreEqual(States.G, state.Name))
                    .Entering(States.B, (state) => Assert.AreEqual(States.B, state.Name))

                    .Exiting(States.R, (state) => Assert.AreEqual(States.R, state.Name))
                    .Exiting(States.G, (state) => Assert.AreEqual(States.G, state.Name))
                    .Exiting(States.B, (state) => Assert.AreEqual(States.B, state.Name))

                    .GoingTo(States.R, States.G, (from, to) => { Assert.AreEqual(States.R, from.Name); Assert.AreEqual(States.G, to.Name); })
                    .GoingTo(States.R, States.B, (from, to) => { Assert.AreEqual(States.R, from.Name); Assert.AreEqual(States.B, to.Name); })
                    .GoingTo(States.G, States.R, (from, to) => { Assert.AreEqual(States.G, from.Name); Assert.AreEqual(States.R, to.Name); })
                    .GoingTo(States.G, States.B, (from, to) => { Assert.AreEqual(States.G, from.Name); Assert.AreEqual(States.B, to.Name); })
                    .GoingTo(States.B, States.R, (from, to) => { Assert.AreEqual(States.B, from.Name); Assert.AreEqual(States.R, to.Name); })
                    .GoingTo(States.B, States.G, (from, to) => { Assert.AreEqual(States.B, from.Name); Assert.AreEqual(States.G, to.Name); })
                ;

            builder.GoToState(States.R);
            builder.GoToState(States.G);
            builder.GoToState(States.B);
        }
    }
}

﻿using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System;
using System.Collections.Generic;

namespace Scripts.Game.Serialized.Brains {

    public class Swarm : PriorityBrain {
        public static readonly Attack ATTACK = new Attack();
        public static readonly EnemyHeal HEAL = new EnemyHeal();
        public static bool spamPreventer, spamPreventer2 = false;
        public int initialEnemiesPresent;
        public bool hasSaidDamagedMessage = false;

        protected override IList<Func<IPlayable>> SetupPriorityPlays() {
            return new Func<IPlayable>[] {
                    CastOnTargetMeetingCondition(HEAL, c => c.Stats.GetMissingStatCount(StatType.HEALTH) > 0),
                    CastOnRandom(ATTACK)
                };
        }

        public override string StartOfRoundDialogue() {
            if (currentBattle.TurnCount == 0 && !spamPreventer) {
                initialEnemiesPresent = allies.Count;
                spamPreventer = true;
                return "We have strength in numbers!/We are many, you are few./You cannnot possibly defeat us together.";
            }
            if (foes.Count < initialEnemiesPresent && !hasSaidDamagedMessage && !spamPreventer2) {
                spamPreventer2 = true;
                hasSaidDamagedMessage = true;
                return "Our defenses are cracking!/Our perfect unity is ruined./We will never be whole again.";
            }
            return string.Empty;
        }
    }

    public class BlackShuck : PriorityBrain {
        public static readonly SetupCounter COUNTER = new SetupCounter();
        public static readonly Attack ATTACK = new Attack();

        protected override IList<Func<IPlayable>> SetupPriorityPlays() {
            return new Func<IPlayable>[] {
                    CastOnRandom(COUNTER),
                    CastOnRandom(ATTACK)
                };
        }

        public override string StartOfRoundDialogue() {
            if (currentBattle.TurnCount == 0) {
                return "(It barks menacingly at you.)";
            }
            return string.Empty;
        }
    }

    public class Siren : BasicBrain {

        public static readonly SpellBook[] DEBUFF_LIST = new SpellBook[] {
            new SingStrengthSong(),
            new SingAgilitySong(),
            new SingIntellectSong(),
            new SingVitalitySong()
        };

        protected override IPlayable GetPlay() {
            return CastOnRandom(DEBUFF_LIST.ChooseRandom());
        }
    }
}
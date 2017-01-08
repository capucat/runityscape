﻿using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Presenter;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Flee : SpellFactory {
        private Page escapePage;

        public Flee(Page escapePage) : base("Flee", "Escape from battle.", SpellType.MERCY, TargetType.SELF) {
            this.escapePage = escapePage;
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                perform: (c, t, calc) => Game.Instance.StartCoroutine(Escape(c)),
                createText: (c, t, calc) => string.Format("{0} flees from battle!", t.DisplayName)
                );
        }

        protected override bool Castable(Character caster, Character target) {
            return !Game.Instance.CurrentPage.GetEnemies(caster).Any(c => c.State == CharacterState.DEFEAT);
        }

        private IEnumerator Escape(Character escapee) {
            escapee.IsCharging = false;
            yield return new WaitForSeconds(0.5f);
            escapee.OnBattleEnd();
            Game.Instance.CurrentPage = escapePage;
        }
    }
}
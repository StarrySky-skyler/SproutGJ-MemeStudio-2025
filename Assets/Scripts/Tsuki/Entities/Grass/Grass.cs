// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 18:02
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections;
using Tsuki.Base;
using Tsuki.Interface;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.Grass
{
    public class Grass : MonoBehaviour, IUndoable
    {
        private static readonly int Break = Animator.StringToHash("Break");
        private Animator _animator;
        private Collider2D _collider2D;
        private int _destroyStep;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            GameManager.Instance.onGameUndo.AddListener(Undo);
        }

        private void OnDisable()
        {
            GameManager.Instance.onGameUndo.RemoveListener(Undo);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Weeders"))
            {
                DebugYumihoshi.Log<Grass>("草实体", "被除草机吃掉了TVT");
                _destroyStep = ModelsManager.Instance.PlayerMod.CurrentLeftStep;
                _animator.SetBool(Break, true);
            }
        }

        public void Undo()
        {
            if (ModelsManager.Instance.PlayerMod.CurrentLeftStep <
                _destroyStep) return;
            DebugYumihoshi.Log<Grass>("草实体", "草因为撤回的生命力被复活了（好耶）");
            StartCoroutine(HandleBug());
        }

        public void DestroySelf()
        {
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
        }

        private IEnumerator HandleBug()
        {
            yield return new WaitForSeconds(0.1f);
            _animator.SetBool(Break, false);
            _spriteRenderer.enabled = true;
            _collider2D.enabled = true;
        }
    }
}

using System.Collections.Generic;
using EvolveUI;
using EvolveUI.UIInput;
using EvolveUI.Util;
using Unity.Mathematics;
using UnityEngine;

namespace HostileMars.UI {


    public class Slider {

        private float _value;
        private float _rangeStart;
        private float _rangeEnd = 100;

        [InjectElementReference] public ElementReference root;
        
        public float Value {
            get => _value;
            set {
                _value = value;
                if (_value < _rangeStart) {
                    _value = _rangeStart;
                }

                if (_value > _rangeEnd) {
                    _value = _rangeEnd;
                }
            }
        }

        public float rangeStart {
            get => _rangeStart;
            set => _rangeStart = value;
        }

        public float rangeEnd {
            get => _rangeEnd;
            set { _rangeEnd = math.max(_rangeStart, value); }
        }

        public float percentage => MathUtil.PercentOfRange(_value, rangeStart, _rangeEnd);


        public void UpdateDragHandle(float2 mouse) {
            float2 localPoint = root.GetLocalPoint(mouse);
            float width = root.GetLayoutSize().width;
            float x = math.clamp(localPoint.x, 0, width);
            _value = MathUtil.RemapRange(x, 0, width, rangeStart, _rangeEnd);
        }

        public void UpdateDragHandleOnClick(float2 mouse) {
            UpdateDragHandle(mouse);
        }

        public DragEvent CreateDrag() {
            return new SliderDragEvent();
        }

        public class SliderDragEvent : DragEvent {}

    }

}
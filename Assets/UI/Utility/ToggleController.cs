using System;
using System.Collections.Generic;
using EvolveUI;
using UnityEngine.Pool;

namespace HostileMars.UI {

    public class ToggleController {

        public int selectedIndex;
        public ToggleController parentController;
        public int firstIndex;
        public int lastIndex;

        private List<ElementReference> selectedElements;
        private List<ElementReference> registeredElements;
        private List<int> registeredIndices;

        private int indexSequence;
        private ElementReference controllerElement;

        public void Setup(ElementReference controllerElement, int sequenceStart = 0) {
            this.controllerElement = controllerElement;
            registeredElements = ListPool<ElementReference>.Get();
            registeredIndices = ListPool<int>.Get();
            indexSequence = sequenceStart;
            firstIndex = sequenceStart;
            lastIndex = sequenceStart;
        }

        public void TearDown() {
            ListPool<ElementReference>.Release(registeredElements);
            ListPool<int>.Release(registeredIndices);
            registeredElements = null;
            registeredIndices = null;
            indexSequence = 0;
        }

        public void RegisterElement(ElementReference el, int? value) {
            indexSequence = value.GetValueOrDefault(indexSequence);
            registeredIndices.Add(indexSequence);
            firstIndex = registeredIndices[0];
            lastIndex = registeredIndices[^1];
            indexSequence++;
            registeredElements.Add(el);
            SetupSelectionState(el);
        }

        private int IndexOf(ElementReference el) {
            for (int index = 0; index < registeredElements.Count; index++) {
                if (registeredElements[index].elementId == el.elementId) {
                    return registeredIndices[index];
                }
            }

            throw new Exception("You made a mistake");
        }

        private ElementReference ElementAtIndex(int elementIndex) {
            for (int index = 0; index < registeredElements.Count; index++) {
                if (registeredIndices[index] == elementIndex) {
                    return registeredElements[index];
                }
            }

            return default;
        }

        public bool IsSelected(ElementReference el) => IndexOf(el) == selectedIndex;

        public ElementReference GetSelectedElement() {
            if (selectedIndex < 0) return default;
            if (selectedIndex >= registeredElements.Count) return default;
            return registeredElements[selectedIndex];
        }

        public void SetupSelectionState(ElementReference el) {
            bool isSelected = IndexOf(el) == selectedIndex;
            bool hasParentSelection = (parentController?.IsSelected(parentController.PeekLastRegisteredElement()) ?? isSelected);
            // If we're nesting ToggleControllers in a tree this ensures that if a parent "branch" is no longer selected, 
            // then this branch should also not be selected anymore. 
            if (parentController != null && !hasParentSelection && selectedIndex > -1) {
                selectedIndex = -1;
            }

            el.SetAttribute("selected", isSelected && hasParentSelection);
            // Special treatment for things that implement ISelectable (see SelectGrid).
            // They hold internal state that needs to be wiped, which should happen inside their Deselect method.
            if (!hasParentSelection && controllerElement.GetCompanion() is ISelectable selectable && selectable.HasSelection()) {
                selectable.Deselect();
            }
        }

        public void Select(ElementReference element) {
            int selectedElementIndex = IndexOf(element);
            if (selectedElementIndex == selectedIndex) {
                return;
            }

            if (selectedIndex > -1) {
                ElementReference elementAtIndex = ElementAtIndex(selectedIndex);
                if (elementAtIndex.elementId.IsValid) {
                    elementAtIndex.SetAttribute("selected", false);
                }
            }

            element.SetAttribute("selected", true);

            selectedIndex = selectedElementIndex;
        }

        public void Select(int index) {
            Select(ElementAtIndex(index));
        }

        public ElementReference PeekLastRegisteredElement() {
            return registeredElements.Count > 0 ? registeredElements[^1] : default;
        }

    }

    public interface ISelectable {

        void Deselect();

        bool HasSelection();

    }

}
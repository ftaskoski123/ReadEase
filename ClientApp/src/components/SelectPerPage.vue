<template>
  <div>
    <select
      v-model="selectedValue"
      @change="handleChange"
      class="form-select block w-32 mt-1 border border-gray-300 rounded-md shadow-sm"
    >
      <option v-for="item in props.itemsPerPageArr" :key="item" :value="item">
        {{ item }}
      </option>
    </select>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const props = defineProps(['itemsPerPage', 'itemsPerPageArr']);
const emits = defineEmits(['update:itemsPerPage']);

const selectedValue = ref(props.itemsPerPage);

function handleChange(event: Event | InputEvent) {
  const target = event.target as HTMLInputElement; 
  selectedValue.value = target.value;
  emits('update:itemsPerPage', selectedValue.value);
}
</script>

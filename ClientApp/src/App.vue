<script setup lang="ts">
import { RouterView } from "vue-router";
import sideBar from "./components/sideBar.vue";
</script>

<template>
  <div class="bg-gray-100 min-h-screen  ">
  <sideBar />
  <RouterView />
</div>
</template>

<style>

::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}

::-webkit-scrollbar-track {
  background-color: #f1f1f1;
  
}

::-webkit-scrollbar-thumb {
  background-color: #888;
  border-radius: 5px;
}

::-webkit-scrollbar-thumb:hover {
  background-color: #555;
}

</style>
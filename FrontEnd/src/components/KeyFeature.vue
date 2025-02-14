<script setup>
  import { ref, computed } from 'vue';

  const props = defineProps({
    feature: Object,
  });

  const showFullDescription = ref(false);
  const truncatedDescription = computed(() => {
    const description = props.feature.description;
    return showFullDescription.value ? description : description.substring(0, 200) + '...';
  });

  const toggleFullDescription = () => {
    showFullDescription.value = !showFullDescription.value;
  };
</script>

<template>
  <div :class="['rounded-xl shadow-md p-4', feature.bgColor]">
    <div class="flex items-center mb-4">
      <i class="pi pi-bookmark-fill mr-5"></i>
      <h3 class="text-xl font-bold text-gray-800">{{ feature.title }}</h3>
    </div>
    <p class="text-gray-600 mb-4">
      {{ truncatedDescription }}
    </p>
    <button @click="toggleFullDescription" class="text-blue-500 hover:text-blue-600">
      {{ showFullDescription ? 'Less' : 'More' }}
    </button>
  </div>
</template>

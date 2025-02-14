<script setup>
  import { RouterLink, useRouter } from 'vue-router';

  const router = useRouter();

  const goBack = () => {
    router.go(-1); // or you can use router.back()
  };
</script>

<template>
  <section class="text-center flex flex-col justify-center items-center h-96">
    <i class="pi pi-exclamation-triangle text-yellow-500 text-7xl mb-5"></i>
    <h1 class="text-6xl font-bold mb-4">404 Not Found</h1>
    <p class="text-xl mb-5">This page does not exist</p>
    <button @click="goBack" class="text-white bg-blue-500 hover:bg-blue-700 rounded-md px-3 py-2 mt-4">
      Go Back
    </button>
  </section>
</template>

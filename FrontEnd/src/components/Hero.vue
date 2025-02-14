<script setup>
  import background from '@/assets/img/background.jpeg';
  import { RouterLink } from 'vue-router';
  defineProps({
    title: {
      type: String,
      default: 'HUSTask',
    },
    subtitle: {
      type: String,
      default:
        'A simple yet powerful web application for personal and team task management, enabling individuals and project leaders to stay organized and monitor task progress effectively.',
    },
  });
</script>

<template>
  <section class="h-[66vh] bg-center bg-no-repeat bg-cover flex items-center justify-center"
    :style="{ backgroundImage: `url(${background})` }">
    <div class="max-w-4xl text-center px-6 sm:px-8 lg:px-12 flex flex-col items-center">
      <h1 class="text-6xl sm:text-7xl font-extrabold text-white leading-tight">
        {{ title }}
      </h1>
      <RouterLink to="/signin"
        class="inline-block bg-blue-500 text-white rounded-full px-4 py-2 hover:bg-blue-600 mt-5">
        Get Started
      </RouterLink>
      <p class="mt-6 text-2xl sm:text-3xl text-white leading-relaxed">
        {{ subtitle }}
      </p>
    </div>
  </section>
</template>

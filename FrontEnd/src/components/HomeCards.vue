<script setup>
  import { RouterLink } from 'vue-router';
  import Card from './Card.vue';
  const userId = localStorage.getItem('userId');
</script>

<template>
  <section class="py-4 border-blue-900">
    <div class="container-xl lg:container m-auto">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 p-4 rounded-lg">
        <!-- Card 1: For Team Members -->
        <Card bg="bg-blue-100">
          <h2 class="text-2xl font-bold">For Team Members</h2>
          <p class="mt-2 mb-4">
            View and update tasks assigned to you to stay on track with your responsibilities.
          </p>
          <RouterLink :to="`/${userId}/project`"
            class="inline-block bg-blue-500 text-white rounded-lg px-4 py-2 hover:bg-blue-600">
            Manage Project Tasks
          </RouterLink>
        </Card>

        <!-- Card 2: For Project Leaders -->
        <Card bg="bg-blue-100">
          <h2 class="text-2xl font-bold">For Project Leaders</h2>
          <p class="mt-2 mb-4">
            Assign tasks to team members and monitor progress to meet project deadlines.
          </p>
          <RouterLink to="/project-overview"
            class="inline-block bg-blue-500 text-white rounded-lg px-4 py-2 hover:bg-blue-600">
            View Projects
          </RouterLink>
        </Card>

        <!-- Card 3: For Individual Users -->
        <Card bg="bg-blue-100">
          <h2 class="text-2xl font-bold">For Individual Users</h2>
          <p class="mt-2 mb-4">
            Manage your personal tasks, set priorities, and track progress to boost productivity.
          </p>
          <RouterLink :to="`/${userId}/all-my-tasks`"
            class="inline-block bg-blue-500 text-white rounded-lg px-4 py-2 hover:bg-blue-600">
            Manage Tasks
          </RouterLink>
        </Card>
      </div>
    </div>
  </section>
</template>

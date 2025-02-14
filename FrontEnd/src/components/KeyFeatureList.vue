<script setup>
  import KeyFeature from './KeyFeature.vue';

  // Features data with unique colors, all using the same icon
  const features = [
    {
      id: 1,
      title: 'Personal Task Management',
      description: 'Create, update, and delete your own tasks. Prioritize them with statuses like Overdue, In Progress, or Done. Set due dates and receive reminders.',
      bgColor: 'bg-blue-100',
    },
    {
      id: 2,
      title: 'Team Task Assignment',
      description: 'Project leaders can create projects, assign tasks, define priorities, and set deadlines with clear instructions for team members.',
      bgColor: 'bg-blue-100',
    },
    {
      id: 3,
      title: 'Task Monitoring',
      description: 'Track task progress using a Kanban-style workflow view. Monitor statuses like Overdue, In Progress, or Done and analyze completion rates.',
      bgColor: 'bg-blue-100',
    },
    {
      id: 4,
      title: 'User Roles',
      description: 'Different roles such as Project Leader, Team Member, and Individual User cater to various needs like task assignment, monitoring, and personal task management.',
      bgColor: 'bg-blue-100',
    },
    {
      id: 5,
      title: 'Notifications and Reminders',
      description: 'Receive timely reminders for deadlines and updates on task progress. Project leaders can get alerts for overdue tasks.',
      bgColor: 'bg-blue-100',
    },
    {
      id: 6,
      title: 'Reporting',
      description: 'Generate reports for task status and team performance. Analyze historical data for completed projects to improve efficiency.',
      bgColor: 'bg-blue-100',
    },
  ];
</script>

<template>
  <section class="bg-white-100 px-4 py-10">
    <div class="container-xl lg:container m-auto">
      <h2 class="text-3xl font-bold text-gray-700 mb-6 text-center">Key Features</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <!-- Iterate over features -->
        <KeyFeature v-for="feature in features" :key="feature.id" :feature="feature" />
      </div>
    </div>
  </section>
</template>

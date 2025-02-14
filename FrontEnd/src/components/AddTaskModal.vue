<script setup>
  import { ref, defineEmits, onMounted } from 'vue';
  import axios from 'axios';
  import postLog from '@/logs/postLog';
  const props = defineProps({
    userId: {
      type: Number,
      required: true
    }
  });

  const emits = defineEmits(['add-task', 'close-modal']);

  const task = ref({
    title: '',
    description: '',
    status: 'pending',
    dueDate: '',
    priorityId: 0,
    userId: props.userId // Automatically set the userId
  });

  const priorities = ref([]);

  const fetchPriorities = async () => {
    try {
      const response = await axios.get('http://localhost:5059/api/Priority');
      priorities.value = response.data;
    } catch (error) {
      console.error('Error fetching priorities:', error);
      alert('Failed to load priorities. Please try again later.');
    }
  };

  onMounted(() => {
    fetchPriorities();
  });

  const submitTask = async () => {
    if (task.value.title && task.value.dueDate && task.value.priorityId) {
      try {
        // 1. First, create the task using TaskController
        const response = await axios.post('http://localhost:5059/api/Task/add', task.value);
        // 2. Then, assign the task to the user via TaskAssignmentController
        const taskAssignment = {
          taskId: response.data.id,
          userId: task.value.userId
        };

        await axios.post('http://localhost:5059/api/TaskAssignment/assign', taskAssignment);
        // Add Log
        const logResponse = await postLog(props.userId, "Create a new task");
        console.log('Log posted:', logResponse);
        // Emit the success event with the added task
        emits('add-task', response.data);
        closeModal();
      } catch (error) {
        console.error('Error adding and assigning task:', error);
        alert('Failed to add and assign task. Please try again.');
      }
    }
  };

  const closeModal = () => {
    emits('close-modal');
  };
</script>

<template>
  <div class="fixed inset-0 bg-gray-800 bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white p-6 rounded-lg shadow-lg w-96">
      <h2 class="text-lg font-bold mb-4">Add New Task</h2>
      <form @submit.prevent="submitTask">
        <!-- Task Title -->
        <div class="mb-4">
          <label for="title" class="block text-sm font-medium text-gray-700">Title</label>
          <input v-model="task.title" type="text" id="title"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            placeholder="Enter task title" required />
        </div>

        <!-- Description -->
        <div class="mb-4">
          <label for="description" class="block text-sm font-medium text-gray-700">Description</label>
          <textarea v-model="task.description" id="description" rows="3"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            placeholder="Enter task description"></textarea>
        </div>

        <!-- Due Date -->
        <div class="mb-4">
          <label for="dueDate" class="block text-sm font-medium text-gray-700">Due Date</label>
          <input v-model="task.dueDate" type="date" id="dueDate"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            required />
        </div>

        <!-- Priority -->
        <div class="mb-4">
          <label for="priority" class="block text-sm font-medium text-gray-700">Priority</label>
          <select v-model="task.priorityId" id="priority"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            required>
            <option value="0" disabled>Select priority</option>
            <option v-for="priority in priorities" :key="priority.id" :value="priority.id">
              {{ priority.title }}
            </option>
          </select>
        </div>

        <div class="flex justify-end space-x-2">
          <button type="button" @click="closeModal"
            class="px-4 py-2 bg-gray-200 rounded-md text-gray-700 hover:bg-gray-300">
            Cancel
          </button>
          <button type="submit" class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
            Add Task
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

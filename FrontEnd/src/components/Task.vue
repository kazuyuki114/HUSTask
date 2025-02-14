<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import postLog from '@/logs/postLog';
  const props = defineProps({
    task: {
      type: Object,
      required: true
    },
  });

  const emit = defineEmits(['edit-task', 'remove-task']);
  const userId = Number(localStorage.getItem('userId'));
  const isFullMode = ref(false);
  const priorities = ref([]);

  const fetchPriorities = async () => {
    try {
      const response = await axios.get('http://localhost:5059/api/priority');
      priorities.value = response.data;
      console.log('Fetched Priorities:', priorities.value); // Debug log
    } catch (error) {
      console.error('Error fetching priorities:', error);
      alert('Failed to load priorities.');
    }
  };

  onMounted(() => {
    fetchPriorities();
    console.log('Task on load:', props.task);
    console.log(props.isLeader);
  });

  const toggleMode = () => {
    isFullMode.value = !isFullMode.value;
  };

  const removeTask = async () => {
    try {
      const taskId = props.task.id;
      await axios.delete(`http://localhost:5059/api/Task/${taskId}`);
      // Add Log
      const logResponse = await postLog(userId, "Removed a task");
      console.log('Log posted:', logResponse);
      emit('remove-task', taskId);
    } catch (error) {
      console.error('Error deleting task:', error.message);
      alert('Failed to delete task');
    }
  };
  const editTask = () => {

    emit('edit-task', props.task); // Emit task to parent for editing
  };

  const getPriorityTitle = (priorityId) => {
    const priority = priorities.value.find(p => p.id === priorityId);
    return priority ? priority.title : 'No priority set';
  };

  // New method to mark the task as done
  const markAsDone = async () => {
    try {
      // Update task status to 'completed'
      const updatedTask = { ...props.task, status: 'completed' };
      // Send the request to update the task on the server
      const response = await axios.put(`http://localhost:5059/api/Task/update/${updatedTask.id}`, updatedTask);
      // Emit the updated task back to the parent component
      emit('edit-task', response.data); // Update the task list in the parent
      // Optionally, update the local task status
      props.task.status = 'completed';
    } catch (error) {
      console.error('Error marking task as done:', error.message);
      alert('Failed to mark task as done');
    }
  };
</script>

<template>
  <div class="task bg-white shadow-md rounded-lg p-4 mb-4">
    <div class="task-header flex justify-between items-center">
      <h3 class="text-lg font-semibold text-gray-700">{{ task.title }}</h3>
      <span class="text-sm text-gray-500">{{ task.dueDate }}</span>
    </div>

    <button v-if="!isFullMode" @click="toggleMode" class="text-blue-500 mt-2">
      See More
    </button>

    <div v-if="isFullMode" class="task-details mt-2">
      <p class="text-sm text-gray-600"><strong>Description:</strong> {{ task.description || 'No description available'
        }}</p>
      <p class="text-sm text-gray-600"><strong>Status:</strong> {{ task.status }}</p>
      <p class="text-sm text-gray-600"><strong>Created At:</strong> {{ task.createdAt }}</p>
      <p class="text-sm text-gray-600"><strong>Updated At:</strong> {{ task.updatedAt }}</p>
      <p class="text-sm text-gray-600"><strong>Priority:</strong> {{ getPriorityTitle(task.priorityId) }}</p>

      <div class="mt-4 flex justify-between items-center">
        <button  @click="editTask" class="text-yellow-500 hover:text-yellow-600">
          Edit
        </button>
        <button  @click="removeTask" class="text-red-500 hover:text-red-600">
          Remove
        </button>
        <button @click="markAsDone" v-if="task.status !== 'completed'" class="text-green-500 hover:text-green-600">
          Done
        </button>
      </div>
    </div>

    <button v-if="isFullMode" @click="toggleMode" class="text-blue-500 mt-2">
      See Less
    </button>
  </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const props = defineProps({
        task: {
            type: Object,
            required: true,
        },
        isLeader: {
            type: Boolean,
            required: false,
        },
    });

    const emit = defineEmits(['edit-task', 'remove-task', 'view-comment']);

    const isFullMode = ref(false);
    const priorities = ref([]);

    // Fetch priorities from the API
    const fetchPriorities = async () => {
        try {
            const response = await axios.get('http://localhost:5059/api/priority');
            priorities.value = response.data;
        } catch (error) {
            console.error('Error fetching priorities:', error.message);
            alert('Failed to load priorities.');
        }
    };

    onMounted(() => {
        fetchPriorities();
    });

    // Toggle full mode to show/hide task details
    const toggleMode = () => {
        isFullMode.value = !isFullMode.value;
    };

    // Emit the task to be removed
    const removeTask = async () => {
        try {
            await axios.delete(`http://localhost:5059/api/Task/${props.task.id}`);
            emit('remove-task', props.task.id);
        } catch (error) {
            console.error('Error deleting task:', error.message);
            alert('Failed to delete task');
        }
    };

    // Emit the task for editing
    const editTask = () => {
        emit('edit-task', props.task);
    };
    const viewComment = () => {
        emit('view-comment', Number(props.task.id)); // Emit task ID to parent
    };


    // Fetch priority title
    const getPriorityTitle = (priorityId) => {
        const priority = priorities.value.find((p) => p.id === priorityId);
        return priority ? priority.title : 'No priority set';
    };

    // Mark the task as done
    const markAsDone = async () => {
        try {
            const updatedTask = { ...props.task, status: 'completed' };
            const response = await axios.put(
                `http://localhost:5059/api/Task/update/${updatedTask.id}`,
                updatedTask
            );
            emit('edit-task', response.data); // Notify parent to update task
        } catch (error) {
            console.error('Error marking task as done:', error.message);
            alert('Failed to mark task as done');
        }
    };
</script>

<template>
    <div class="task bg-white shadow-md rounded-lg p-4 mb-4">
        <!-- Task Header -->
        <div class="task-header flex justify-between items-center">
            <h3 class="text-lg font-semibold text-gray-700">{{ task.title }}</h3>
            <span class="text-sm text-gray-500">{{ task.dueDate || 'No due date' }}</span>
        </div>

        <!-- Toggle Details Button -->
        <button v-if="!isFullMode" @click="toggleMode" class="text-blue-500 mt-2">
            See More
        </button>

        <!-- Task Details -->
        <div v-if="isFullMode" class="task-details mt-2">
            <p class="text-sm text-gray-600"><strong>Description:</strong> {{ task.description }}</p>
            <p class="text-sm text-gray-600"><strong>Status:</strong> {{ task.status }}</p>
            <p class="text-sm text-gray-600"><strong>Priority:</strong> {{ getPriorityTitle(task.priorityId) }}</p>

            <!-- Action Buttons -->
            <div class="mt-4 flex justify-between items-center">
                <button v-if="isLeader" @click="editTask" class="text-yellow-500 hover:text-yellow-600">
                    Edit
                </button>
                <button v-if="isLeader" @click="removeTask" class="text-red-500 hover:text-red-600">
                    Remove
                </button>
                <button @click="viewComment" class="text-blue-500 hover:text-blue-600">
                    View Comment
                </button>
                <button @click="markAsDone" v-if="task.status !== 'completed'"
                    class="text-green-500 hover:text-green-600">
                    Done
                </button>
            </div>
        </div>

        <!-- Toggle Back Button -->
        <button v-if="isFullMode" @click="toggleMode" class="text-blue-500 mt-2">
            See Less
        </button>
    </div>
</template>

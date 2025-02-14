<script setup>
    import { computed, onMounted, ref } from 'vue';

    // Define props for the comment object
    const props = defineProps({
        comment: {
            type: Object,
            required: true,
        },
        currentUser: {
            type: Number,
            required: false,
        }
    });
    // Owner
    const isOwner = ref(false);
    // Check owner function
    const checkOwner = async () => {
        if (props.currentUser == props.comment.userId) {
            isOwner.value = true;
        }
    }
    // Emit event to notify parent component when the card is closed
    const emit = defineEmits(['delete-comment']);

    // Compute the user's display name and email, with fallback values if unavailable
    const userName = computed(() => props.comment.commentedUser?.name || 'Unknown User');
    const userEmail = computed(() => props.comment.commentedUser?.email || 'No Email');
    onMounted(checkOwner);
</script>

<template>
    <div class="bg-white rounded-lg shadow-md p-4 border border-gray-200 relative">
        <!-- Close Button -->
        <button v-if="isOwner" class="absolute top-2 right-2 text-gray-500 hover:text-gray-800"
            @click="$emit('delete-comment')" aria-label="Close">
            &times;
        </button>

        <!-- Header Section -->
        <div class="flex items-center space-x-4 border-b pb-2 mb-2">
            <div class="flex flex-col">
                <h3 class="text-lg font-semibold text-gray-800">{{ userName }}</h3>
                <p class="text-sm text-gray-500">{{ userEmail }}</p>
            </div>
        </div>

        <!-- Comment Content -->
        <div class="text-gray-700">
            <p>{{ props.comment.content }}</p>
        </div>
    </div>
</template>

<style scoped>
    button {
        background: none;
        border: none;
        font-size: 1.25rem;
        cursor: pointer;
    }
</style>

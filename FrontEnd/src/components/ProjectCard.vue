<template>
    <div class="card bg-white rounded-lg shadow-md overflow-hidden">
        <!-- Card Image Slot -->
        <div class="card-image" v-if="$slots.image">
            <slot name="image"></slot>
        </div>

        <!-- Card Header -->
        <div class="card-header p-4 text-left">
            <h4 class="card-title text-lg font-bold truncate">{{ groupCode }}</h4>
            <p class="card-category text-sm text-gray-500 truncate">{{ groupName }}</p>
        </div>

        <!-- Card Body -->
        <div class="card-body p-4 text-left">
            <slot></slot> <!-- Default slot for additional content -->
        </div>

        <!-- Card Footer -->
        <div class="card-footer p-4 text-left" v-if="$slots.footer">
            <hr />
            <slot name="footer"></slot>
        </div>
    </div>
</template>

<script setup>
    defineProps({
        projectId: {
            type: Number,
            required: true,
        },
        groupCode: {
            type: String,
            required: true,
        },
        groupName: {
            type: String,
            required: true,
        },
    });
</script>

<style scoped>
    .card {
        max-width: 300px;
    }

    .card-image {
        height: 150px;
        background-color: #f3f4f6;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .card-title {
        margin-bottom: 0.5rem;
    }

    .card-category {
        color: #6b7280;
    }
</style>

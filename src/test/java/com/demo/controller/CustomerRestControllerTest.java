package com.demo.controller;

import com.demo.model.Customer;
import com.demo.repository.CustomerRestRepository;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import java.util.Arrays;
import java.util.Optional;

import static org.hamcrest.Matchers.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyLong;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

@WebMvcTest(CustomerRestController.class)
class CustomerRestControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockBean
    private CustomerRestRepository customerRepository;

    @Test
    @DisplayName("GET /api/customers - returns list of customers")
    void getAllCustomers_returnsList() throws Exception {
        Customer c1 = new Customer(1L, "John", "Doe", "john.doe@example.com");
        Customer c2 = new Customer(2L, "Jane", "Smith", "jane.smith@example.com");
        Mockito.when(customerRepository.findAll()).thenReturn(Arrays.asList(c1, c2));

        mockMvc.perform(get("/api/customers"))
                .andExpect(status().isOk())
                .andExpect(content().contentType(MediaType.APPLICATION_JSON))
                .andExpect(jsonPath("$", hasSize(2)))
                .andExpect(jsonPath("[0].id", is(1)))
                .andExpect(jsonPath("[0].firstName", is("John")))
                .andExpect(jsonPath("[1].id", is(2)))
                .andExpect(jsonPath("[1].email", is("jane.smith@example.com")));
    }

    @Test
    @DisplayName("GET /api/customers/{id} - returns existing customer")
    void getCustomerById_existing_returnsOk() throws Exception {
        Customer c = new Customer(10L, "Alice", "Wonder", "alice@example.com");
        Mockito.when(customerRepository.findById(10L)).thenReturn(Optional.of(c));

        mockMvc.perform(get("/api/customers/10"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id", is(10)))
                .andExpect(jsonPath("$.firstName", is("Alice")));
    }

    @Test
    @DisplayName("GET /api/customers/{id} - returns 404 when not found")
    void getCustomerById_notFound_returns404() throws Exception {
        Mockito.when(customerRepository.findById(anyLong())).thenReturn(Optional.empty());

        mockMvc.perform(get("/api/customers/999"))
                .andExpect(status().isNotFound());
    }

    @Test
    @DisplayName("POST /api/customers - creates a new customer")
    void createCustomer_returnsCreated() throws Exception {
        Customer toCreate = new Customer(null, "Bob", "Builder", "bob.builder@example.com");
        Customer saved = new Customer(5L, "Bob", "Builder", "bob.builder@example.com");
        Mockito.when(customerRepository.save(any(Customer.class))).thenReturn(saved);

        mockMvc.perform(post("/api/customers")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(toCreate)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.id", is(5)))
                .andExpect(jsonPath("$.firstName", is("Bob")));
    }

    @Test
    @DisplayName("PUT /api/customers/{id} - updates existing customer")
    void updateCustomer_existing_returnsOk() throws Exception {
        Customer existing = new Customer(7L, "Old", "Name", "old@example.com");
        Customer updated = new Customer(7L, "New", "Name", "new@example.com");
        Mockito.when(customerRepository.findById(7L)).thenReturn(Optional.of(existing));
        Mockito.when(customerRepository.save(any(Customer.class))).thenReturn(updated);

        mockMvc.perform(put("/api/customers/7")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(updated)))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.firstName", is("New")))
                .andExpect(jsonPath("$.email", is("new@example.com")));
    }

    @Test
    @DisplayName("DELETE /api/customers/{id} - deletes existing customer")
    void deleteCustomer_existing_returnsNoContent() throws Exception {
        Mockito.when(customerRepository.existsById(3L)).thenReturn(true);
        Mockito.doNothing().when(customerRepository).deleteById(3L);

        mockMvc.perform(delete("/api/customers/3"))
                .andExpect(status().isNoContent());
    }

    @Test
    @DisplayName("DELETE /api/customers/{id} - returns 404 when customer does not exist")
    void deleteCustomer_notFound_returns404() throws Exception {
        Mockito.when(customerRepository.existsById(anyLong())).thenReturn(false);

        mockMvc.perform(delete("/api/customers/999"))
                .andExpect(status().isNotFound());
    }
}

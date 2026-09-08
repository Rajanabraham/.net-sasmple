package com.demo.repository;

import com.demo.model.Customer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

/**
 * Repository interface for {@link Customer} entity.
 * Extends {@link JpaRepository} to inherit standard CRUD operations.
 */
@Repository
public interface CustomerSpringDataRepository extends JpaRepository<Customer, Long> {
    // Additional query methods can be defined here if needed.
}
